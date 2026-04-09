import base64
import json
import os
import socket

HOST = "127.0.0.1"
PORT = 5000


def send_json_line(conn: socket.socket, payload: dict) -> None:
    message = json.dumps(payload) + "\n"
    conn.sendall(message.encode("utf-8"))


def recv_json_line(conn: socket.socket) -> dict:
    data = b""
    while not data.endswith(b"\n"):
        chunk = conn.recv(4096)
        if not chunk:
            raise ConnectionError("Server disconnected before sending full response")
        data += chunk
    return json.loads(data.decode("utf-8").strip())


def collect_customer_data() -> dict:
    print("=== EasyDrive Customer Registration ===")

    name = input("Enter Name: ").strip()
    address = input("Enter Address: ").strip()
    pps_number = input("Enter PPS Number: ").strip()

    print("\nDriving License Document:")
    print("- Enter a file path to upload the document")
    print("- Or press Enter to type document details manually")
    license_path = input("License file path (optional): ").strip().strip('"')

    payload = {
        "name": name,
        "address": address,
        "pps_number": pps_number,
        "license_filename": None,
        "license_file": None,
        "license_text": None,
    }

    if license_path:
        if not os.path.isfile(license_path):
            print("File not found. Switching to manual license details entry.")
            payload["license_text"] = input("Enter driving license details: ").strip()
        else:
            with open(license_path, "rb") as file:
                file_bytes = file.read()
            payload["license_filename"] = os.path.basename(license_path)
            payload["license_file"] = base64.b64encode(file_bytes).decode("utf-8")
    else:
        payload["license_text"] = input("Enter driving license details: ").strip()

    return payload


def register_customer() -> None:
    customer_data = collect_customer_data()

    request = {
        "type": "register",
        "data": customer_data,
    }

    try:
        with socket.socket(socket.AF_INET, socket.SOCK_STREAM) as client:
            print(f"\nConnecting to server {HOST}:{PORT}...")
            client.connect((HOST, PORT))
            print("Connection established.")

            send_json_line(client, request)
            response = recv_json_line(client)

            if response.get("status") == "ok":
                print("\nRegistration Successful")
                print("Your unique registration number is:")
                print(response.get("registration_number"))
            else:
                print("\nRegistration Failed")
                print("Reason:", response.get("message"))

    except ConnectionRefusedError:
        print("\nCould not connect to server. Ensure Que4_server.py is running.")
    except Exception as exc:
        print(f"\nAn error occurred: {exc}")


if __name__ == "__main__":
    register_customer()
