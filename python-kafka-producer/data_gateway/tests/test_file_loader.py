import pathlib
import unittest

from data_gateway.file_loader import FileLoader


class TestFileLoader(unittest.TestCase):

    def test_apply_with_valid_input_expect_correct_results(self):
        # Arrange
        expected_output_end = str(pathlib.Path().absolute().parent.joinpath(f'data'))

        # Act
        FileLoader.set_up_class()
        received_output = FileLoader.data_path

        # Assert
        self.assertTrue(received_output.endswith(expected_output_end))

    def test_get_country_codes_csv_path(self):
        # Arrange
        expected_output_end = '/country_codes.csv'

        # Act
        received_output = FileLoader.get_country_codes_csv_path()

        # Assert
        self.assertTrue(received_output.endswith(expected_output_end))

    def test_get_country_codes_csv(self):
        # Act
        received_output = FileLoader.get_country_codes_csv()
        count = len(received_output)

        # Assert
        self.assertGreater(count, 0)