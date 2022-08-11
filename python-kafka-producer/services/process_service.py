import traceback

from data_gateway.file_loader import FileLoader
from infrastructure.consts import Consts
from infrastructure.guard import Guard
from infrastructure.log import Logger
from kafka_utils.message_producer import MessageProducer
from services.message_factory import MessageFactory


class ProcessService:
    def __init__(self, message_producer: MessageProducer):
        Guard.argument_not_null(message_producer)
        self.message_producer = message_producer
        self.logger = Logger.get_logger()

    def run(self):
        try:
            self.logger.info(Consts.READ_AND_PRODUCE_RUNNING)
            data = FileLoader.get_country_codes_csv()
            final_df_row_count = len(data)

            self.logger.info(Consts.GENERATING_MESSAGES)
            final_message = MessageFactory.build_message(data)

            self.logger.info(Consts.PROCESSING_MESSAGES)
            for message_dto in final_message:
                self.message_producer.send_message(message_dto, final_df_row_count)
            self.logger.info(Consts.MESSAGES_GENERATION_FINISHED)

        except Exception as err:
            self.logger.error(Consts.EXCEPTION_MESSAGE)
