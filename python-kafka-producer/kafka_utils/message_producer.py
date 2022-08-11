import threading
import traceback
from typing import Callable

from confluent_kafka import KafkaError, Message

from infrastructure.consts import Consts
from infrastructure.guard import Guard
from infrastructure.log import Logger
from kafka_contracts.message import Message
from kafka_contracts.message_dto import MessageDto
from kafka_utils.gzip_helper import GzipHelper
from kafka_utils.json_helper import JsonHelper
from kafka_utils.kafka_aio_producer import KafkaAIOProducer
from kafka_utils.kafka_configuration import KafkaConfiguration
from kafka_utils.report import Report

message_success_counter = 0
message_failure_counter = 0
total_messages_sent = 0
message_failure_errors = str()
lock = threading.Lock()


class MessageProducer:
    def __init__(self,
                 kafka_configuration: KafkaConfiguration,
                 kafka_topic_producer: KafkaAIOProducer,
                 logger: Logger = None):
        Guard.argument_not_null_or_empty(kafka_configuration)
        Guard.argument_not_null_or_empty(kafka_topic_producer)
        self.kafka_configuration = kafka_configuration
        self.kafka_topic_producer = kafka_topic_producer
        self.logger = logger.get_logger()

    def send_message(self, message_dto: MessageDto,
                     final_df_row_count: int, message_counter_threshold: int = 0):
        try:
            current_message = Message(
                self.kafka_configuration.kafka_topic.version,
                message_dto)
            json_result = JsonHelper.serialize(current_message)
            self.kafka_topic_producer.produce(
                topic=self.kafka_configuration.kafka_topic.topic_name,
                value=GzipHelper.gzip_bytes(json_result),
                on_delivery=self._build_on_delivery(current_message,
                                                    self.logger,
                                                    final_df_row_count,
                                                    message_counter_threshold))
        except Exception as err:
            self.logger.error(Consts.EXCEPTION_MESSAGE)

    @staticmethod
    def _build_on_delivery(message: Message,
                           logger: Logger,
                           final_df_row_count: int,
                           message_counter_threshold: int = 0) -> Callable[[KafkaError, Message], None]:
        def on_delivery(error: KafkaError, message1: Message):
            report = Report(message, message1, error)
            data = JsonHelper.serialize(report)

            global message_failure_counter
            global message_success_counter
            global message_failure_errors
            global total_messages_sent

            lock.acquire()
            total_messages_sent += 1
            if error:
                message_failure_counter += 1
                message_failure_errors += Consts.SENT_MESSAGE_ERROR_DATA.format(message_failure_counter,
                                                                                error,
                                                                                data)
            else:
                message_success_counter += 1
            lock.release()

            if total_messages_sent < (final_df_row_count - message_counter_threshold):
                return
            logger.info(Consts.MESSAGES_COUNTER.format(total_messages_sent, final_df_row_count))

            if message_failure_counter > 0:
                logger.error(Consts.MESSAGES_ERROR_COUNTER.format(message_failure_counter,
                                                                  total_messages_sent,
                                                                  message_failure_errors))

            if message_success_counter >= (final_df_row_count - message_counter_threshold):
                logger.info(Consts.ALL_MESSAGES_SUCCESS)
            else:
                logger.info(Consts.MESSAGES_SUCCESS_COUNTER.format(message_success_counter, final_df_row_count))

            lock.acquire()
            if total_messages_sent >= (final_df_row_count - message_counter_threshold) \
                    or message_success_counter > 0 \
                    or message_failure_counter > 0 \
                    or message_failure_errors != str():
                total_messages_sent = 0
                message_success_counter = 0
                message_failure_counter = 0
                message_failure_errors = str()
            lock.release()

        return on_delivery
