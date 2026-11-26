# Coffee Break API

API minimale in .NET 10 per la pausa caffè: espone il menu delle bevande e permette di ordinarle.
Gli ordini sono conservati solo in memoria (nessun database) e si perdono al riavvio.

## Avvio

```bash
dotnet run --project src/CoffeeBreak.Api
```

L'API ascolta su `http://localhost:5080`. Nel file `CoffeeBreak.Api.http` ci sono richieste di esempio.

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
  { "id": "decaffeinato", "name": "Decaffeinato", "price": 1.40 }
]
```

### `POST /orders`

Corpo: `drinkId` (obbligatorio, un `id` del menu) e `quantity` (opzionale, intero ≥ 1, default 1).

```bash
curl -i -X POST http://localhost:5080/orders \
  -H "Content-Type: application/json" \
  -d '{"drinkId":"cappuccino","quantity":2}'
```

Risposta `201 Created` (con header `Location`):

```json
{
  "id": "3209753c-7574-4f9e-b00f-669eab6a55a1",
  "drinkId": "cappuccino",
  "drinkName": "Cappuccino",
  "quantity": 2,
  "total": 3.60,
  "createdAt": "2026-11-26T18:14:28.2953868+00:00"
}
```

Un `drinkId` sconosciuto o mancante, oppure una `quantity` minore di 1, risponde `400 Bad Request`.
