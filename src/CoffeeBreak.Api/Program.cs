using System.Collections.Concurrent;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<OrderStore>();

var app = builder.Build();

app.MapGet("/menu", () => Results.Ok(Menu.Drinks));

app.MapPost("/orders", (CreateOrderRequest request, OrderStore store) =>
{
    var drink = Menu.Drinks.FirstOrDefault(d => d.Id == request.DrinkId);
    if (drink is null)
    {
        return Results.BadRequest(new { error = $"Bevanda sconosciuta: '{request.DrinkId}'." });
    }

    var quantity = request.Quantity ?? 1;
    if (quantity < 1)
    {
        return Results.BadRequest(new { error = "La quantità deve essere almeno 1." });
    }

    var order = store.Add(drink, quantity);
    return Results.Created($"/orders/{order.Id}", order);
});

await app.RunAsync();

internal sealed record Drink(string Id, string Name, decimal Price);

internal sealed record CreateOrderRequest(string? DrinkId, int? Quantity);

internal sealed record Order(Guid Id, string DrinkId, string DrinkName, int Quantity, decimal UnitPrice, decimal Total, DateTimeOffset CreatedAt);

internal static class Menu
{
    public static IReadOnlyList<Drink> Drinks { get; } =
    [
        new("espresso", "Espresso", 1.20m),
        new("cappuccino", "Cappuccino", 1.80m),
        new("decaffeinato", "Decaffeinato", 1.50m),
    ];
}

internal sealed class OrderStore
{
    private readonly ConcurrentDictionary<Guid, Order> _orders = new();

    public Order Add(Drink drink, int quantity)
    {
        var order = new Order(Guid.NewGuid(), drink.Id, drink.Name, quantity, drink.Price, drink.Price * quantity, DateTimeOffset.UtcNow);
        _orders[order.Id] = order;
        return order;
    }
}
