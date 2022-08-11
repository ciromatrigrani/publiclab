import asyncio
from asyncio import AbstractEventLoop
from threading import Thread
from typing import Callable

from confluent_kafka import KafkaError, Message, Producer

from infrastructure.guard import Guard


class KafkaAIOProducer:
    def __init__(self, configs: dict, loop: AbstractEventLoop = None):
        """Wrapper class for :class:`confluent_kafka.Producer`. It starts a thread in background to execute poll, so
        that on_delivery function is executed on both success and failure scenarios. Note: asyncio method not
        implemented yet.

            Example:
                producer = KafkaAIOProducer({'bootstrap.servers': 'localhost:12345'})

            Args:
                configs (dict): Dictionary with kafka producer configs.
                loop (:class:`asyncio.events.AbstractEventLoop`): custom EventLoop.

            Raises:
                ValueError: if configs parameter is None or empty
        """
        Guard.argument_not_null_or_empty(configs)

        self._loop = loop or asyncio.get_event_loop()
        self._producer = Producer(configs)
        self._cancelled = False
        self._poll_thread = Thread(target=self._poll_loop, daemon=True)
        self._poll_thread.start()

    def _poll_loop(self) -> None:
        while not self._cancelled:
            self._producer.poll(timeout=5)

    def close(self) -> None:
        self._cancelled = True
        self._poll_thread.join()

    def produce(self, topic: str, value: bytes, on_delivery: Callable[[KafkaError, Message], None] = None):
        self._producer.produce(topic, value, callback=on_delivery)
