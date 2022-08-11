import unittest

import pandas as pd

from kafka_contracts.message_dto import MessageDto
from services.message_factory import MessageFactory


class TestMessageFactory(unittest.TestCase):

    def test_build_message_expect_correct_results(self):
        # Arrange
        raw_input = pd.DataFrame({
            'country': ['Afghanistan'],
            'country_code': ['af'],
            'language': ['en']
        })
        expected_output = set()
        expected_output.add(MessageDto('Afghanistan', 'af', 'en'))

        # Act
        received_output = MessageFactory.build_message(raw_input)

        # Assert
        self.assertEqual(len(expected_output), len(received_output))
