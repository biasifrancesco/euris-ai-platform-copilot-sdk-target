using System.Collections.Concurrent;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<OrderStore>();

var app = builder.Build();

app.MapGet("/menu", () => Menu.Drinks);

app.MapPost("/orders", (CreateOrderRequest? request, OrderStore store) =>
{
    var drink = Menu.Drinks.FirstOrDefault(d => d.Id == request?.DrinkId);
    if (drink is null)
    {
        return Results.BadRequest(new { error = "drinkId sconosciuto." });
    }

    var order = store.Add(drink);
    return Results.Created($"/orders/{order.Id}", order);
});

app.MapGet("/orders/{id:guid}", (Guid id, OrderStore store) =>
    store.Find(id) is { } order ? Results.Ok(order) : Results.NotFound());

app.Run();

internal sealed record Drink(string Id, string Name, decimal Price);

internal sealed record CreateOrderRequest(string? DrinkId);

internal sealed record Order(Guid Id, string DrinkId, string DrinkName, decimal Price, DateTimeOffset CreatedAt);

internal static class Menu
{
    public static IReadOnlyList<Drink> Drinks { get; } =
    [
        new("espresso", "Espresso", 1.20m),
        new("cappuccino", "Cappuccino", 1.80m),
        new("latte-macchiato", "Latte macchiato", 2.20m),
        new("caffe-americano", "Caffè americano", 1.50m),
    ];
}

internal sealed class OrderStore
{
    private readonly ConcurrentDictionary<Guid, Order> _orders = new();

    public Order Add(Drink drink)
    {
        var order = new Order(Guid.NewGuid(), drink.Id, drink.Name, drink.Price, DateTimeOffset.UtcNow);
        _orders[order.Id] = order;
        return order;
    }

    public Order? Find(Guid id) => _orders.GetValueOrDefault(id);
}
