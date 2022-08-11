import unittest
from unittest.mock import Mock

from confluent_kafka import KafkaError

from kafka_utils.error_codes import ErrorCodes
from kafka_utils.report_error import ReportError


class TestReportError(unittest.TestCase):

    def test_report_error_no_parameters(self):
        # arrange + act
        report_error = ReportError()

        # assert
        self.assertEqual(ErrorCodes.NO_ERROR, report_error.error_code)
        self.assertFalse(report_error.has_error)
        self.assertEqual('', report_error.inner_error)
        self.assertFalse(report_error.is_broker_error)
        self.assertFalse(report_error.is_local_error)

    def test_report_error_broker_exception(self):
        # arrange
        error_str = 'a broker exception occurred'
        broker_error = Mock(spec=KafkaError)
        broker_error.str.return_value = error_str
        broker_error.code.return_value = 100

        # act
        report_error = ReportError(error=broker_error)

        # assert
        self.assertEqual(ErrorCodes.NOT_MAPPED_CHECK_INNER_ERROR, report_error.error_code)
        self.assertTrue(report_error.has_error)
        self.assertEqual(error_str, report_error.inner_error)
        self.assertTrue(report_error.is_broker_error)
        self.assertFalse(report_error.is_local_error)

    def test_report_error_local_exception(self):
        # arrange
        error_str = 'a local exception occurred'
        local_error = Mock(spec=KafkaError)
        local_error.str.return_value = error_str
        local_error.code.return_value = -100

        # act
        report_error = ReportError(error=local_error)

        # assert
        self.assertEqual(ErrorCodes.NOT_MAPPED_CHECK_INNER_ERROR, report_error.error_code)
        self.assertTrue(report_error.has_error)
        self.assertEqual(error_str, report_error.inner_error)
        self.assertFalse(report_error.is_broker_error)
        self.assertTrue(report_error.is_local_error)
