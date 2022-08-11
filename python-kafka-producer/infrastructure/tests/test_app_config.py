import unittest

from infrastructure.app_config import AppConfig
from infrastructure.configuration import config
from infrastructure.consts import Consts


class TestAppConfig(unittest.TestCase):
    def test_load_success(self):
        # Act
        app_config = AppConfig(config)

        # Assert
        self.assertEqual(config['app_config']['server'], app_config.server)
        self.assertEqual(config['app_config']['name'], app_config.name)
        self.assertEqual(config['app_config']['port'], app_config.port)
        self.assertEqual(config['app_config']['scheduler_config']['cron_time'], app_config.cron_time)
        self.assertEqual(config['app_config']['scheduler_config']['cron_time_enabled'], app_config.cron_time_enabled)


