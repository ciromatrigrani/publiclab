import unittest
from unittest.mock import patch, ANY

import jsonpickle as pickle

from infrastructure.configuration import config
from infrastructure.log import Logger
from kafka_contracts.message import Message
from kafka_contracts.message_dto import MessageDto
from kafka_utils.gzip_helper import GzipHelper
from kafka_utils.kafka_aio_producer import KafkaAIOProducer
from kafka_utils.kafka_configuration import KafkaConfiguration
from kafka_utils.message_producer import MessageProducer


class TestMessageBus(unittest.TestCase):

    @patch('kafka_utils.kafka_aio_producer.KafkaAIOProducer')
    @patch('kafka_utils.kafka_configuration')
    def test_message_producer_success(self, producer, kafka_configuration):
        # Act
        message_producer = MessageProducer(kafka_configuration, producer, Logger.get_logger())

        # Assert
        self.assertIsInstance(message_producer, MessageProducer)

    @patch('kafka_utils.kafka_configuration')
    def test_message_producer_producer_null(self, kafka_configuration):
        # Act, Assert
        self.assertRaises(ValueError, MessageProducer, kafka_configuration, None)

    @patch('kafka_utils.kafka_aio_producer.KafkaAIOProducer')
    def test_message_producer_kafka_configuration_null(self, producer):
        # Act, Assert
        self.assertRaises(ValueError, MessageProducer, None, producer)

    @patch('kafka_utils.kafka_aio_producer.KafkaAIOProducer')
    @patch('infrastructure.log.Logger')
    def test_send_message_success(self, mock_producer: KafkaAIOProducer, logger: Logger):
        # Arrange
        kafka_configuration = KafkaConfiguration(config)
        message_producer = MessageProducer(kafka_configuration, mock_producer, logger)
        document = MessageDto("Brazil", "br", "pt")
        message = self.get_full_message(
            kafka_configuration.kafka_topic.version,
            document)

        # Act
        message_producer.send_message(document, 1)

        # Assert
        mock_producer.produce.assert_called_with(topic=kafka_configuration.kafka_topic.topic_name,
                                                 value=GzipHelper.gzip_bytes(message),
                                                 on_delivery=ANY)
        logger.error.assert_not_called()

    @staticmethod
    def get_full_message(version, document) -> str:
        message = Message(
            version,
            document)
        return pickle.encode(message, False)
