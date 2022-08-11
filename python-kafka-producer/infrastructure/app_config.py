from typing import Dict, Any


class AppConfig:
    def __init__(self, configurations: Dict[str, Any]):
        app_config_root = configurations['app_config']
        self.name = app_config_root['name']
        self.server = app_config_root['server']
        self.port = app_config_root['port']
        scheduler_config = app_config_root['scheduler_config']
        self.cron_time = scheduler_config['cron_time']
        self.cron_time_enabled = scheduler_config['cron_time_enabled']