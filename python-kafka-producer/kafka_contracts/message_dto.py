from infrastructure.guard import Guard


class MessageDto:
    def __init__(self,
                 country: str,
                 country_code: str,
                 language: str):
        """
            Args:
                country (str)
                country_code (str)
                language (str)
            Raises:
                ValueError: if some of the params are null or empty
        """
        Guard.argument_not_null_or_empty(country)
        Guard.argument_not_null_or_empty(language)
        Guard.argument_not_null_or_empty(country_code)

        self.country = country
        self.country_code = country_code
        self.language = language

