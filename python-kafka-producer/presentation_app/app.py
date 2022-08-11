import datetime

from apscheduler.schedulers.blocking import BlockingScheduler
from tzlocal import get_localzone

from infrastructure.app_config import AppConfig
from infrastructure.configuration import config
from infrastructure.consts import Consts
from infrastructure.log import Logger
from kafka_utils.kafka_setup import KafkaSetup
from presentation_app.setup_cron import SetupCron


def main():
    app_config = AppConfig(config)
    kafka_setup = KafkaSetup()
    logger = Logger.get_logger()
    logger.info(f"{Consts.SERVICE_STARTING_AT}{datetime.datetime.now()}")
    scheduler = BlockingScheduler(timezone=get_localzone())

    try:
        setup_cron = SetupCron(kafka_setup, scheduler)
        setup_cron.configure_cron(app_config)

        scheduler.start()
    except:
        logger.error(Consts.EXCEPTION_MESSAGE)
    kafka_setup.kafka_topic_producer.close()
    scheduler.remove_all_jobs()
    scheduler.shutdown()


if __name__ == '__main__':
    main()
