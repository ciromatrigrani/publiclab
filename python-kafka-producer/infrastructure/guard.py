class Guard:
    @staticmethod
    def argument_not_null(argument_value):
        if argument_value is None:
            raise ValueError

    @staticmethod
    def argument_not_null_or_empty(argument_value):
        Guard.argument_not_null(argument_value)
        if type(argument_value) in (str, int):
            Guard.str_not_empty(argument_value)
        if type(argument_value) in (dict, list):
            Guard.structure_not_empty(argument_value)

    @staticmethod
    def str_not_empty(argument_value):
        if not argument_value:
            raise ValueError

    @staticmethod
    def structure_not_empty(argument_value):
        if not argument_value or len(argument_value) == 0:
            raise ValueError

    @staticmethod
    def arguments_at_least_one_not_null_or_empty(*argument_values):
        for argument_value in argument_values:
            if argument_value:
                return True
        raise ValueError
