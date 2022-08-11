from typing import Any

import jsonpickle
import numpy as np


class JsonHelper:
    """Helper class to serialize complex objects to json"""

    @staticmethod
    def serialize(generic_object: Any) -> str:
        """Serializes a generic complex object into a json string.
        Parameters:
            generic_object (Any): Object to serialize

        Returns:
            Serialized object.
        """
        return jsonpickle.encode(generic_object, False)


class NumpyIntHandler(jsonpickle.handlers.BaseHandler):
    def restore(self, obj):
        pass

    def flatten(self, obj, data):
        return int(obj)


jsonpickle.handlers.registry.register(np.int, NumpyIntHandler)
jsonpickle.handlers.registry.register(np.int8, NumpyIntHandler)
jsonpickle.handlers.registry.register(np.int16, NumpyIntHandler)
jsonpickle.handlers.registry.register(np.int32, NumpyIntHandler)
jsonpickle.handlers.registry.register(np.int64, NumpyIntHandler)
jsonpickle.handlers.registry.register(np.uint8, NumpyIntHandler)
jsonpickle.handlers.registry.register(np.uint16, NumpyIntHandler)
jsonpickle.handlers.registry.register(np.uint32, NumpyIntHandler)
jsonpickle.handlers.registry.register(np.uint64, NumpyIntHandler)
