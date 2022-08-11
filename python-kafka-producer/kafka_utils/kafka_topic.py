from typing import Dict, Any


class KafkaTopic:
    def __init__(self, configs: Dict[str, Any]):
        self.topic_name = configs['topic_name']
        self.message_type = configs['message_type']
        self.stabilization_time = configs['stabilization_time']
        self.version = configs['version']
        producer = configs['producer_config']
        self.message_timeout_ms = producer['message_timeout_ms']
        self.message_send_max_retries = producer['message_send_max_retries']