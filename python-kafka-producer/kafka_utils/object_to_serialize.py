from typing import Any


class ObjectToSerialize:
    """Helper class to use on JsonHelper unit tests"""
    def __init__(self, a: str, b: int, c: Any):
        self.a = a
        self.b = b
        self.c = c
