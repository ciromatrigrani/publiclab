import pathlib
from pathlib import Path
import pandas as pd


class FileLoader:
    """

    """
    data_path = None

    def __init__(self):
        self.set_up_class()

    @classmethod
    def set_up_class(cls) -> None:
        """
        Sets up data_path to the data folder within domain_model_ai
        """
        cls.data_path = str(pathlib.Path(
            f'{Path(__file__).parent.absolute()}/data/').absolute()) \
            if cls.data_path is None \
            else cls.data_path

    @classmethod
    def get_country_codes_csv_path(cls):
        cls.set_up_class()
        return cls.data_path + '/country_codes_lang.csv'

    @classmethod
    def get_country_codes_csv(cls) -> pd.DataFrame:
        return pd.read_csv(cls.get_country_codes_csv_path())

