from confluent_kafka import KafkaError

from kafka_utils.error_codes import ErrorCodes


class ReportError:
    def __init__(self, error: KafkaError = None):
        if error is None:
            self.error_code: int = ErrorCodes.NO_ERROR
            self.has_error: bool = False
            self.inner_error: str = ''
            self.is_broker_error: bool = False
            self.is_local_error: bool = False
        else:
            self.error_code: int = ErrorCodes.NOT_MAPPED_CHECK_INNER_ERROR
            self.has_error: bool = True
            self.inner_error: str = error.str()
            self.is_broker_error: bool = error.code() > 0
            self.is_local_error: bool = error.code() < -1
