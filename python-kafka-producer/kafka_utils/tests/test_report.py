import unittest
from unittest.mock import Mock

from confluent_kafka import KafkaError, Message

from kafka_utils.report import Report
from kafka_utils.report_error import ReportError


class TestReport(unittest.TestCase):

    def test_report_default_parameters(self):
        # arrange
        kafka_message = self._mock_kafka_message()
        payload = 'dummy payload'

        # act
        report = Report(payload=payload, kafka_message=kafka_message)

        # assert
        self.assertEqual(ReportError().__dict__, report.error.__dict__)
        self.assertEqual('', report.key)
        self.assertEqual(-1, report.offset)
        self.assertEqual(-1, report.partition)
        self.assertEqual('', report.topic_name)
        self.assertEqual(payload, report.message)

    def test_report_non_default_parameters(self):
        # arrange
        key = 'ABCDEFGH123456'
        offset = 123
        partition = 456
        topic = "a_kafka_dummy_topic_name"
        kafka_message = self._mock_kafka_message(key=key, offset=offset, partition=partition, topic=topic)

        kafka_error = Mock(spec=KafkaError)
        kafka_error.str.return_value = 'an error message'
        kafka_error.code.return_value = 10

        payload = 'dummy payload'

        # act
        report = Report(payload=payload, kafka_message=kafka_message, error=kafka_error)

        # assert
        self.assertEqual(ReportError(kafka_error).__dict__, report.error.__dict__)
        self.assertEqual(key, report.key)
        self.assertEqual(offset, report.offset)
        self.assertEqual(partition, report.partition)
        self.assertEqual(topic, report.topic_name)
        self.assertEqual(payload, report.message)

    @staticmethod
    def _mock_kafka_message(key: str = None, offset: int = None, partition: int = None, topic: str = None) -> Mock:
        mock_message = Mock(spec=Message)
        mock_message.key.return_value = key
        mock_message.offset.return_value = offset
        mock_message.partition.return_value = partition
        mock_message.topic.return_value = topic
        return mock_message
