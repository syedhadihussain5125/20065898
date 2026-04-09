import csv
from pathlib import Path

import requests
from bs4 import BeautifulSoup

URL = "https://books.toscrape.com/catalogue/category/books/travel_2/index.html"
OUTPUT_CSV = Path("travel_books.csv")
RATING_MAP = {
    "One": 1,
    "Two": 2,
    "Three": 3,
    "Four": 4,
    "Five": 5,
}


def scrape_books(url: str) -> list[dict[str, str | int]]:
    response = requests.get(url, timeout=30)
    response.raise_for_status()
    response.encoding = "utf-8"

    soup = BeautifulSoup(response.text, "html.parser")
    books = []

    for article in soup.select("article.product_pod"):
        title_tag = article.select_one("h3 a")
        rating_tag = article.select_one("p.star-rating")
        price_tag = article.select_one("p.price_color")

        title = title_tag["title"].strip() if title_tag and title_tag.has_attr("title") else ""

        rating_word = ""
        if rating_tag:
            for class_name in rating_tag.get("class", []):
                if class_name in RATING_MAP:
                    rating_word = class_name
                    break

        rating = RATING_MAP.get(rating_word, 0)
        price = price_tag.get_text(strip=True) if price_tag else ""

        books.append(
            {
                "book_name": title,
                "rating": rating,
                "price": price,
            }
        )

    return books


def save_to_csv(rows: list[dict[str, str | int]], csv_path: Path) -> None:
    with csv_path.open("w", newline="", encoding="utf-8") as file:
        writer = csv.DictWriter(file, fieldnames=["book_name", "rating", "price"])
        writer.writeheader()
        writer.writerows(rows)


def read_from_csv(csv_path: Path) -> list[dict[str, str]]:
    with csv_path.open("r", newline="", encoding="utf-8") as file:
        reader = csv.DictReader(file)
        return list(reader)


def print_rows(rows: list[dict[str, str]]) -> None:
    print("\nData read from CSV:\n")
    for index, row in enumerate(rows, start=1):
        print(f"{index}. Book Name: {row['book_name']} | Rating: {row['rating']} | Price: {row['price']}")


def main() -> None:
    books = scrape_books(URL)
    save_to_csv(books, OUTPUT_CSV)

    csv_data = read_from_csv(OUTPUT_CSV)
    print_rows(csv_data)


if __name__ == "__main__":
    main()
