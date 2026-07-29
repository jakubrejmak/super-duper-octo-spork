# Generyczny parser danych w ASP.NET, Minimal API

## Wymagania

.NET Runtime 10.0

## Uruchomienie

W folderze projektu (z plikiem .slnx) uruchom polecenie:
`dotnet run`

## Specyfikacja API

### Endpoint POST `/parse-content`

Content-Type: `application/json`

Body requestu powinno miec nastepujaca strukture:

```JSON
{
    "type": "INTERNAL_JSON" | "CSV",
    "content": "<BASE64>"
}
```

#### Klucz `type` - pole wymagane

Dopuszczalne wartości:

- CSV
- INTERNAL_JSON

#### Klucz `content` - pole wymagane

Powinno zawierać ciąg znaków zakodowany w formacie Base64:

- dla CSV – dane w formacie CSV,
- dla INTERNAL_JSON – dokument JSON.

W przypadku INTERNAL_JSON zdekodowany dokument JSON musi być tablicą obiektów.

#### Odpowiedź

Dane wejściowe są dekodowane z formatu Base64, a następnie
przekształcane do natywnych obiektów .NET.
Wynik jest zwracany jako tablica obiektów JSON.

Typ INTERNAL_JSON
Wartości właściwości są konwertowane do odpowiednich
typów CLR zgodnie z logiką metody JSONParser.ConvertValue().

Typ CSV
Każda wartość jest zwracana jako string.
Nazwy kolumn stają się nazwami właściwości obiektów w odpowiedzi.

### Przykład użycia

```BASH
curl -X POST http://localhost:5167/api/v1/parse-content \
  -H "Content-Type: application/json" \
  -d '{
    "type": "CSV",
    "content": "bmFtZSxhZ2UsY2l0eQpBbGljZSwzMCxMb25kb24KQm9iLDI1LFBhcmlz"
  }'
```

```BASH
curl -X POST http://localhost:5167/api/v1/parse-content \
  -H "Content-Type: application/json" \
  -d '{
    "type": "INTERNAL_JSON",
    "content": "W3siaWQiOiIxIiwibmFtZSI6IkFsaWNlIiwiYWN0aXZlIjoidHJ1ZSIsInNjb3JlIjoiOTguNSJ9LHsiaWQiOiIyIiwibmFtZSI6IkJvYiIsImFjdGl2ZSI6ImZhbHNlIiwic2NvcmUiOiI3NSJ9XQ=="
  }'
```
