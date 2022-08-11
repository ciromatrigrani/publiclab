import unittest

from infrastructure.guard import Guard

# TODO: implement more tests for guard and helpers
class TestGuard(unittest.TestCase):

    def test_guard__arguments_at_least_one_not_null__success(self):
        # Act
        val = Guard.arguments_at_least_one_not_null_or_empty(None, None, 1, None)

        # Assert
        self.assertTrue(val)

    def test_guard__arguments_at_least_one_not_null__str_empty__raises_value_error(self):
        # Act, Assert
        self.assertRaises(ValueError, Guard.arguments_at_least_one_not_null_or_empty, None, None, str(), None)

    def test_guard__arguments_at_least_one_not_null__all_null__raises_value_error(self):
        # Act, Assert
        self.assertRaises(ValueError, Guard.arguments_at_least_one_not_null_or_empty, None, None, None, None)
