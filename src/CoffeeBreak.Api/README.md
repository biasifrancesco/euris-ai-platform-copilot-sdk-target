# CoffeeBreak.Api

API minimale in .NET 10 per la pausa caffè: espone il menu delle bevande e permette di ordinarne una indicando la quantità.
Gli ordini sono conservati solo in memoria (nessun database) e si perdono al riavvio.

## Avvio

```bash
dotnet run --project src/CoffeeBreak.Api
```

L'API ascolta su `http://localhost:5080`. Le stesse richieste sono disponibili in [`CoffeeBreak.Api.http`](CoffeeBreak.Api.http).

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
  { "id": "decaffeinato", "name": "Decaffeinato", "price": 1.30 }
]
```

### `POST /orders`

Corpo: `drinkId` (obbligatorio, un `id` del menu) e `quantity` (opzionale, intero ≥ 1, default 1).

```bash
curl -i -X POST http://localhost:5080/orders \
  -H "Content-Type: application/json" \
  -d '{"drinkId": "cappuccino", "quantity": 2}'
```

Risposta `201 Created` (header `Location: /orders/{id}`):

```json
{
  "id": "1e70fc76-a9a3-45d0-a5f0-5f612e9ca148",
  "drinkId": "cappuccino",
  "drinkName": "Cappuccino",
  "quantity": 2,
  "total": 3.60,
  "createdAt": "2026-11-27T19:02:26.8421027+00:00"
}
```

Un `drinkId` sconosciuto o una `quantity` minore di 1 restituiscono `400 Bad Request`:

```bash
curl -i -X POST http://localhost:5080/orders \
  -H "Content-Type: application/json" \
  -d '{"drinkId": "tè"}'
```

### `GET /orders/{id}`

Restituisce l'ordine creato (`200 OK`) oppure `404 Not Found`.
