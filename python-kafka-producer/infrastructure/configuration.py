import yaml
from pathlib import Path
from typing import Any, Dict


def _get_settings() -> dict:
    configurations_path = find_config_file(name='appconfig.yaml')
    with open(configurations_path, 'r') as file:
        return yaml.load(file)


def find_config_file(name: str) -> str:
    possible_paths = ['./configuration', '../configuration', '../presentation_app/configuration', '../../presentation_app/configuration', '../../../presentation_app/configuration']

    for directory in possible_paths:
        if directory is not None:
            file_path = Path(directory).joinpath(name)
            if file_path.exists():
                return str(file_path)

    raise IOError(f"Couldn't find {name} in any of the following folders: {possible_paths}")


config: Dict[str, Any] = _get_settings()
