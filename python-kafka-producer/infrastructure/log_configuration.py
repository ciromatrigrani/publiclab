from typing import Dict, Any


class LogConfiguration:
    def __init__(self, configs: Dict[str, Any]):
        self.location = configs['log_config']["location"]
        self.verbose = configs['log_config']["verbose"]
        self.enable = configs['log_config']["enable"]
