import base64
import json
import socket
import sqlite3
import threading
from datetime import datetime
from pathlib import Path

HOST = "127.0.0.1"
PORT = 5000
DB_FILE = Path(__file__).resolve().parent / "easydrive.db"


def init_db() -> None:
    conn = sqlite3.connect(DB_FILE)
    cursor = conn.cursor()
    cursor.execute(
        """
        CREATE TABLE IF NOT EXISTS customers (
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            registration_number TEXT UNIQUE,
            name TEXT NOT NULL,
            address TEXT NOT NULL,
            pps_number TEXT NOT NULL,
            license_filename TEXT,
            license_blob BLOB,
            license_text TEXT,
            created_at TEXT NOT NULL
        )
        """
    )
    conn.commit()
    conn.close()


def recv_json_line(conn: socket.socket) -> dict:
    data = b""
    while not data.endswith(b"\n"):
        chunk = conn.recv(4096)
        if not chunk:
            raise ConnectionError("Client disconnected before sending full message")
        data += chunk
    return json.loads(data.decode("utf-8").strip())


def send_json_line(conn: socket.socket, payload: dict) -> None:
    message = json.dumps(payload) + "\n"
    conn.sendall(message.encode("utf-8"))


def generate_registration_number(row_id: int) -> str:
    today = datetime.now().strftime("%Y%m%d")
    return f"ED-{today}-{row_id:06d}"


def insert_customer(payload: dict) -> str:
    name = payload.get("name", "").strip()
    address = payload.get("address", "").strip()
    pps_number = payload.get("pps_number", "").strip()

    if not name or not address or not pps_number:
        raise ValueError("Name, Address, and PPS Number are required.")

    license_filename = None
    license_blob = None
    license_text = None

    if payload.get("license_file"):
        license_filename = payload.get("license_filename")
        b64_data = payload.get("license_file")
        try:
            license_blob = base64.b64decode(b64_data)
        except Exception as exc:
            raise ValueError("Invalid license file encoding.") from exc
    else:
        license_text = payload.get("license_text", "").strip()

    created_at = datetime.now().isoformat(timespec="seconds")

    conn = sqlite3.connect(DB_FILE)
    cursor = conn.cursor()

    cursor.execute(
        """
        INSERT INTO customers (
            registration_number, name, address, pps_number,
            license_filename, license_blob, license_text, created_at
        ) VALUES (?, ?, ?, ?, ?, ?, ?, ?)
        """,
        (None, name, address, pps_number, license_filename, license_blob, license_text, created_at),
    )

    row_id = cursor.lastrowid
    registration_number = generate_registration_number(row_id)

    cursor.execute(
        "UPDATE customers SET registration_number = ? WHERE id = ?",
        (registration_number, row_id),
    )

    conn.commit()
    conn.close()

    return registration_number


def handle_client(conn: socket.socket, addr) -> None:
    try:
        print(f"[+] Connected by {addr}")
        request = recv_json_line(conn)

        if request.get("type") != "register":
            raise ValueError("Unsupported request type.")

        registration_number = insert_customer(request.get("data", {}))
        send_json_line(
            conn,
            {
                "status": "ok",
                "registration_number": registration_number,
                "message": "Registration successful.",
            },
        )
        print(f"[+] Registration completed for {addr}. Reg No: {registration_number}")

    except Exception as exc:
        send_json_line(conn, {"status": "error", "message": str(exc)})
        print(f"[!] Error handling {addr}: {exc}")
    finally:
        conn.close()
        print(f"[-] Connection closed for {addr}")


def start_server() -> None:
    init_db()

    server = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    server.setsockopt(socket.SOL_SOCKET, socket.SO_REUSEADDR, 1)
    server.bind((HOST, PORT))
    server.listen(5)

    print(f"EasyDrive server is listening on {HOST}:{PORT}")
    print(f"Database file: {DB_FILE.resolve()}")

    while True:
        conn, addr = server.accept()
        client_thread = threading.Thread(target=handle_client, args=(conn, addr), daemon=True)
        client_thread.start()


if __name__ == "__main__":
    start_server()
