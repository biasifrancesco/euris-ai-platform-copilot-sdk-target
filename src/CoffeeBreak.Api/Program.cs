using System.Collections.Concurrent;
using CoffeeBreak.Api;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

Drink[] menu =
[
    new("espresso", "Espresso", 1.20m),
    new("cappuccino", "Cappuccino", 1.80m),
    new("decaffeinato", "Decaffeinato", 1.30m),
];

// Gli ordini vivono solo in memoria e si perdono al riavvio.
ConcurrentDictionary<Guid, Order> orders = new();

app.MapGet("/menu", () => Results.Ok(menu));

app.MapPost("/orders", (CreateOrderRequest request) =>
{
    var drink = menu.FirstOrDefault(d => d.Id == request.DrinkId);
    if (drink is null)
    {
        return Results.Problem(
            title: "Bevanda sconosciuta",
            detail: $"Nessuna bevanda con drinkId '{request.DrinkId}'.",
            statusCode: StatusCodes.Status400BadRequest);
    }

    var quantity = request.Quantity ?? 1;
    if (quantity < 1)
    {
        return Results.Problem(
            title: "Quantità non valida",
            detail: "La quantità deve essere almeno 1.",
            statusCode: StatusCodes.Status400BadRequest);
    }

    var order = new Order(Guid.NewGuid(), drink.Id, drink.Name, quantity, drink.Price * quantity, DateTimeOffset.UtcNow);
    orders[order.Id] = order;
    return Results.Created($"/orders/{order.Id}", order);
});

app.MapGet("/orders/{id:guid}", (Guid id) =>
    orders.TryGetValue(id, out var order) ? Results.Ok(order) : Results.NotFound());

await app.RunAsync();
