import unittest

from infrastructure.log_configuration import LogConfiguration
from infrastructure.configuration import config


class TestLogConfiguration(unittest.TestCase):
    def test_load_success(self):
        # Act
        log_configuration = LogConfiguration(config)

        # Assert
        self.assertEqual(config['log_config']['enable'], log_configuration.enable)
        self.assertEqual(config['log_config']['location'], log_configuration.location)
        self.assertEqual(config['log_config']['verbose'], log_configuration.verbose)
