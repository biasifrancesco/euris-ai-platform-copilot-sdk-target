# CoffeeBreak.Api

API minimale .NET per la pausa caffè: mostra le bevande disponibili e permette di ordinarne una.
Gli ordini sono tenuti solo in memoria e si perdono al riavvio.

## Avvio

```powershell
dotnet run --project src/CoffeeBreak.Api
```

L'app ascolta su <http://localhost:5080>.

## Endpoint

### `GET /menu`

```bash
curl http://localhost:5080/menu
```

Risposta `200 OK`:

```json
[
  { "id": "espresso", "name": "Espresso", "price": 1.20 },
  { "id": "cappuccino", "name": "Cappuccino", "price": 1.80 },
  { "id": "latte-macchiato", "name": "Latte macchiato", "price": 2.20 },
  { "id": "caffe-americano", "name": "Caffè americano", "price": 1.50 }
]
```

### `POST /orders`

```bash
curl -i -X POST http://localhost:5080/orders \
  -H "Content-Type: application/json" \
  -d '{"drinkId": "espresso"}'
```

Risposta `201 Created` (con header `Location`):

```json
{
  "id": "1cb23c8d-677b-4bf8-8371-0939792bfd59",
  "drinkId": "espresso",
  "drinkName": "Espresso",
  "price": 1.20,
  "createdAt": "2026-11-26T16:16:46.2688966+00:00"
}
```

Un `drinkId` sconosciuto (o mancante) risponde `400 Bad Request`:

```json
{ "error": "drinkId sconosciuto." }
```

### `GET /orders/{id}`

Restituisce `200 OK` con l'ordine, oppure `404` se non esiste.

Richieste di esempio pronte all'uso sono in `CoffeeBreak.Api.http`.
