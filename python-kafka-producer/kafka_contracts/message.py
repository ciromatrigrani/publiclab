from infrastructure.guard import Guard
from kafka_contracts.message_dto import MessageDto


class Message:
    def __init__(self, version: str, message_dto: MessageDto):
        """
            Args:
                version (str): message version
                message_dto (MessageDto): document to be add in the Kafka message

            Raises:
                ValueError: if some of the params are null or empty
        """
        Guard.argument_not_null_or_empty(version)
        Guard.argument_not_null_or_empty(message_dto)

        self.version = version
        self.message_schema = message_dto
