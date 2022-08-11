import pandas as pd

from kafka_contracts.message_dto import MessageDto


class MessageFactory:

    @staticmethod
    def build_message(final_data: pd.DataFrame) -> set:
        """
        Parameters
        ----------
        final_data : Pandas DataFrame
        Returns
        -------
        full_message : Set[MessageDto]
                     documents to be add in the Kafka message
        """
        full_message = set()
        for i in range(len(final_data)):
            row = final_data.iloc[i]
            message = MessageDto(row.country, row.country_code, row.language)
            full_message.add(message)
        return full_message
