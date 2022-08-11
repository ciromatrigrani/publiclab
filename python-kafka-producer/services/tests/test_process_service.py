import unittest
from unittest.mock import patch

from kafka_utils.message_producer import MessageProducer
from services.process_service import ProcessService


class TestProcessService(unittest.TestCase):

    @patch('kafka_utils.message_producer.MessageProducer')
    def test_process_service(self, message_producer: MessageProducer):
        # Act, Assert
        process = ProcessService(message_producer)

        self.assertIsNotNone(process)

    def test_process_message_processor_null(self):
        # Act, Assert
        self.assertRaises(
            ValueError,
            ProcessService,
            None)

