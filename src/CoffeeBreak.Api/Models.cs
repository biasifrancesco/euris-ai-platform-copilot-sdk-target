namespace CoffeeBreak.Api;

internal sealed record Drink(string Id, string Name, decimal Price);

internal sealed record CreateOrderRequest(string? DrinkId, int? Quantity);

internal sealed record Order(Guid Id, string DrinkId, string DrinkName, int Quantity, decimal Total, DateTimeOffset CreatedAt);
