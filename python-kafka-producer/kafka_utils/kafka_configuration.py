from typing import Dict, Any

from infrastructure.consts import Consts
from kafka_utils.kafka_topic import KafkaTopic


class KafkaConfiguration:
    def __init__(self, config: Dict[str, Any]):
        self.server = config['kafka_config']['broker_node'] + ":" + str(config['kafka_config']['broker_port'])
        self.pause_interval_in_ms = config['kafka_config']['pause_interval_ms']
        self.kafka_topic = KafkaTopic(config['kafka_config']['topics_config'])
