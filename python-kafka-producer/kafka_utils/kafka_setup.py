from kafka_utils.kafka_aio_producer import KafkaAIOProducer
from infrastructure.configuration import config
from kafka_utils.kafka_configuration import KafkaConfiguration


class KafkaSetup:
    def __init__(self):
        self.kafka_configuration = KafkaConfiguration(config)
        self.kafka_topic_producer = self._get_kafka_topic_producer(self.kafka_configuration)

    @staticmethod
    def _get_kafka_topic_producer(configuration: KafkaConfiguration) -> KafkaAIOProducer:
        conf = {
            'bootstrap.servers': configuration.server,
            'message.send.max.retries': configuration.kafka_topic.message_send_max_retries,
            'message.timeout.ms': configuration.kafka_topic.message_timeout_ms
        }
        return KafkaAIOProducer(conf)
