using System.Collections.Concurrent;
using CoffeeBreak.Api;

Drink[] menu =
[
    new("espresso", "Espresso", 1.20m),
    new("cappuccino", "Cappuccino", 1.80m),
    new("decaffeinato", "Decaffeinato", 1.40m),
];

ConcurrentDictionary<Guid, Order> orders = new();

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
WebApplication app = builder.Build();

app.MapGet("/menu", () => Results.Ok(menu));

app.MapPost("/orders", (CreateOrderRequest? request) =>
{
    Drink? drink = menu.FirstOrDefault(d => string.Equals(d.Id, request?.DrinkId, StringComparison.Ordinal));
    if (drink is null)
    {
        return Results.Problem(
            detail: "drinkId sconosciuto o mancante.",
            statusCode: StatusCodes.Status400BadRequest);
    }

    int quantity = request?.Quantity ?? 1;
    if (quantity < 1)
    {
        return Results.Problem(
            detail: "quantity deve essere almeno 1.",
            statusCode: StatusCodes.Status400BadRequest);
    }

    Order order = new(Guid.NewGuid(), drink.Id, drink.Name, quantity, drink.Price * quantity, DateTimeOffset.UtcNow);
    orders[order.Id] = order;
    return Results.Created($"/orders/{order.Id}", order);
});

await app.RunAsync();
