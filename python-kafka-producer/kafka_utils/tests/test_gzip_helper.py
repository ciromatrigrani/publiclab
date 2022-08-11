import unittest

from kafka_contracts.message import Message
from kafka_contracts.message_dto import MessageDto
from kafka_utils.gzip_helper import GzipHelper
from kafka_utils.json_helper import JsonHelper


class TestGzipHelper(unittest.TestCase):

    def test_gzip_bytes(self):
        # Arrange
        message_dto = MessageDto("Brazil", "br", "pt")
        message = Message("1.0", message_dto)
        json = JsonHelper.serialize(message)
        # act
        result = GzipHelper.gzip_bytes(json)
        result1 = GzipHelper.gzip_bytes(json)
        # assert
        self.assertEqual(result, result1)
