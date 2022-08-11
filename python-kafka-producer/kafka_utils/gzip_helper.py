import gzip
import io


class GzipHelper:
    @staticmethod
    def gzip_bytes(string: str) -> bytes:
        out = io.BytesIO()

        with gzip.GzipFile(fileobj=out, mode='w') as file:
            file.write(string.encode())

        bytes_obj = out.getvalue()
        return bytes_obj
