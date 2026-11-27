using CoffeeBreak.Api;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<OrderStore>();
builder.Services.AddSingleton(TimeProvider.System);

var app = builder.Build();

Drink[] menu =
[
    new("espresso", "Espresso", 1.20m),
    new("cappuccino", "Cappuccino", 1.80m),
    new("decaffeinato", "Decaffeinato", 1.30m),
];

app.MapGet("/menu", () => Results.Ok(menu));

app.MapGet("/orders/{id:guid}", (Guid id, OrderStore store) =>
    store.Find(id) is { } order ? Results.Ok(order) : Results.NotFound());

app.MapPost("/orders", (CreateOrderRequest? request, OrderStore store, TimeProvider clock) =>
{
    var drink = Array.Find(menu, d => string.Equals(d.Id, request?.DrinkId, StringComparison.OrdinalIgnoreCase));
    if (drink is null)
    {
        return Results.BadRequest(new { error = "drinkId sconosciuto" });
    }

    var quantity = request?.Quantity ?? 1;
    if (quantity < 1)
    {
        return Results.BadRequest(new { error = "quantity deve essere almeno 1" });
    }

    var order = store.Add(drink, quantity, clock);
    return Results.Created($"/orders/{order.Id}", order);
});

await app.RunAsync();
