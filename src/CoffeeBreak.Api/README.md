# CoffeeBreak.Api

API minimale .NET 10 per la pausa caffè: espone il menu delle bevande e permette di ordinarne una.
Gli ordini sono conservati solo in memoria (nessun database) e si perdono al riavvio.

## Avvio

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
  { "id": "decaffeinato", "name": "Decaffeinato", "price": 1.50 }
]
```

### `POST /orders`

Crea un ordine per una bevanda del menu. `quantity` è facoltativo (default 1, minimo 1); un valore minore di 1 risponde `400`. Risponde `201 Created` con l'ordine e l'header `Location`.

```bash
curl -i -X POST http://localhost:5080/orders \
  -H "Content-Type: application/json" \
  -d '{"drinkId": "cappuccino", "quantity": 2}'
```

```json
{
  "id": "3f0c5c0e-6a1c-4c53-9a53-0f2f5a6f8d11",
  "drinkId": "cappuccino",
  "drinkName": "Cappuccino",
  "quantity": 2,
  "unitPrice": 1.80,
  "total": 3.60,
  "createdAt": "2026-11-27T14:10:00+00:00"
}
```

Un `drinkId` non presente nel menu risponde `400 Bad Request`:

```json
{ "error": "Bevanda sconosciuta: 'tè'." }
```

Altri esempi di richieste sono in [`CoffeeBreak.Api.http`](CoffeeBreak.Api.http).
