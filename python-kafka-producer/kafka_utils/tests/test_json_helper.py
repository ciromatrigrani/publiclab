import unittest

from kafka_utils.json_helper import JsonHelper
from kafka_utils.object_to_serialize import ObjectToSerialize


class TestJsonHelper(unittest.TestCase):

    def test_serialize(self):
        # arrange
        to_serialize = ObjectToSerialize("a1", 1, ObjectToSerialize("a2", 2, None))

        # act
        json = JsonHelper.serialize(to_serialize)

        # assert
        self.assertEqual('{"a": "a1", "b": 1, "c": {"a": "a2", "b": 2, "c": null}}', json)
