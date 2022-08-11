import unittest

from kafka_utils.kafka_aio_producer import KafkaAIOProducer


class TestKafkaAIOProducer(unittest.TestCase):

    def test_kafka_aio_producer_configs_null(self):
        # Act, Assert
        self.assertRaises(ValueError, KafkaAIOProducer, configs=None)

    def test_kafka_aio_producer_configs_empty(self):
        # Act, Assert
        self.assertRaises(ValueError, KafkaAIOProducer, configs={})

    def test_kafka_aio_producer_loop_default(self):
        # arrange
        configs = {'bootstrap.servers': 'localhost:12345'}

        # act
        producer = KafkaAIOProducer(configs=configs, loop=None)

        # assert
        self.assertIsNotNone(producer._loop)
