# Coffee Break API

API minimale per la pausa caffè: mostra il menu delle bevande e permette di ordinarle.
Gli ordini sono conservati solo in memoria (nessun database) e si perdono al riavvio.

## Avvio

Dalla root del repository (serve .NET 10 SDK):

```bash
dotnet run --project src/CoffeeBreak.Api
```

L'API ascolta su `http://localhost:5080`.

## Endpoint

### `GET /menu`

```bash
curl http://localhost:5080/menu
```

```json
[
  { "id": "espresso", "name": "Espresso", "price": 1.20 },
  { "id": "cappuccino", "name": "Cappuccino", "price": 1.80 },
  { "id": "decaffeinato", "name": "Decaffeinato", "price": 1.30 }
]
```

### `POST /orders`

Corpo: `drinkId` (obbligatorio, un `id` del menu) e `quantity` (opzionale, intero ≥ 1, predefinito 1).

```bash
curl -i -X POST http://localhost:5080/orders \
  -H "Content-Type: application/json" \
  -d '{"drinkId": "cappuccino", "quantity": 2}'
```

Risposta `201 Created` (con header `Location`):

```json
{
  "id": "3f0c6a52-6a0e-4a43-9d57-0a1f3d3c2b11",
  "drinkId": "cappuccino",
  "drinkName": "Cappuccino",
  "quantity": 2,
  "total": 3.60,
  "createdAt": "2026-11-27T12:00:00+00:00"
}
```

Un `drinkId` sconosciuto (o una `quantity` minore di 1) risponde `400 Bad Request`:

```json
{ "error": "drinkId sconosciuto" }
```

### `GET /orders/{id}`

Restituisce un ordine creato in precedenza (`200`), oppure `404` se non esiste.

## Richieste di esempio

Vedi [`CoffeeBreak.Api.http`](CoffeeBreak.Api.http).
