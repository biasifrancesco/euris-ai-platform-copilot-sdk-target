namespace CoffeeBreak.Api;

public sealed record Drink(string Id, string Name, decimal Price);

public sealed record CreateOrderRequest(string? DrinkId, int? Quantity);

public sealed record Order(Guid Id, string DrinkId, string DrinkName, int Quantity, decimal Total, DateTimeOffset CreatedAt);
