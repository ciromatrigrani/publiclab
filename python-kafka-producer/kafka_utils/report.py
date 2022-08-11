from typing import TypeVar, Generic

from confluent_kafka import KafkaError, Message

from kafka_utils.report_error import ReportError

T = TypeVar('T')


class Report(Generic[T]):
    def __init__(self, payload: T, kafka_message: Message, error: KafkaError = None):
        self.error: ReportError = ReportError(error)
        self.key: str = str(kafka_message.key()) if kafka_message.key() is not None else ''
        self.offset: int = kafka_message.offset() if kafka_message.offset() else -1
        self.partition: int = kafka_message.partition() if kafka_message.partition() else -1
        self.topic_name: str = kafka_message.topic() if kafka_message.topic() else ''
        self.message: T = payload
