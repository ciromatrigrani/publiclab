import datetime

from apscheduler.schedulers.blocking import BlockingScheduler
from apscheduler.triggers.cron import CronTrigger

from infrastructure.consts import Consts
from services.process_service import ProcessService
from kafka_utils.message_producer import MessageProducer
from infrastructure.app_config import AppConfig
from infrastructure.log import Logger
from kafka_utils.kafka_setup import KafkaSetup


class SetupCron:
    def __init__(self, kafka_setup: KafkaSetup, scheduler: BlockingScheduler):
        self.scheduler = scheduler
        self.logger = Logger.get_logger()
        self.message_producer = MessageProducer(kafka_setup.kafka_configuration,
                                           kafka_setup.kafka_topic_producer,
                                           self.logger)

    def configure_cron(self, app_config: AppConfig):
        if app_config.cron_time_enabled:
            process = ProcessService(self.message_producer)

            cron_time = self.get_cron_time(app_config.cron_time)
            self.add_cron(cron_time, app_config.cron_time_enabled,  process)

    def add_cron(self, cron_time, cron_time_enabled, process):
        self.logger.info(f"{Consts.CRON_ENABLED_AT}{cron_time}")

        if not cron_time_enabled:
            return

        if cron_time == "false":
            self.scheduler.add_job(process.run)
            return

        self.scheduler.add_job(process.run, CronTrigger.from_crontab(cron_time))

    @staticmethod
    def get_cron_time(cron_time):
        if cron_time == str():
            start_time = datetime.datetime.now() + datetime.timedelta(minutes=1)

            return f'{start_time.minute} {start_time.hour} * *' \
                   + f' {start_time.weekday()}'

        return cron_time
