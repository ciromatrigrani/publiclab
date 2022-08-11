from infrastructure.consts import Consts
from infrastructure.log_configuration import LogConfiguration


class Logger:
    def __new__(cls, config):
        if not hasattr(cls, 'instance'):
            cls.instance = super(Logger, cls).__new__(cls, config)
        return cls.instance

    def __init__(self, config: LogConfiguration):
        self.enable = config.enable
        self.location = config.location
        self.verbose = config.verbose

    @classmethod
    def get_logger(cls):
        return cls

    @staticmethod
    def error(message: str):
        print(Consts.LOG_ERROR + message)

    @staticmethod
    def warning(message: str):
        print(Consts.LOG_WARNING + message)

    @staticmethod
    def info(message: str):
        print(Consts.LOG_INFO + message)

    @staticmethod
    def verbose(cls, message: str):
        print(Consts.LOG_VERBOSE + message)

