import unittest

from infrastructure.configuration import config
from kafka_utils.kafka_configuration import KafkaConfiguration


class TestKafkaConfiguration(unittest.TestCase):
    def test_load_success(self):
        # Arrange
        server = config['kafka_config']['broker_node'] + ":" + str(config['kafka_config']['broker_port'])

        # Act
        kafka_configuration = KafkaConfiguration(config)

        # Assert

        self.assertEqual(server, kafka_configuration.server)
        self.assertEqual(config['kafka_config']['pause_interval_ms'], kafka_configuration.pause_interval_in_ms)

        topics = config['kafka_config']['topics_config']
        self.assertEqual(topics['topic_name'], kafka_configuration.kafka_topic.topic_name)
        self.assertEqual(topics['version'], kafka_configuration.kafka_topic.version)
        self.assertEqual(topics['stabilization_time'], kafka_configuration.kafka_topic.stabilization_time)
        producer_config = topics['producer_config']
        self.assertEqual(producer_config['message_timeout_ms'], kafka_configuration.kafka_topic.message_timeout_ms)
        self.assertEqual(producer_config['message_send_max_retries'], kafka_configuration.kafka_topic.message_send_max_retries)


