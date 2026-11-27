using System.Collections.Concurrent;

namespace CoffeeBreak.Api;

public sealed class OrderStore
{
    private readonly ConcurrentDictionary<Guid, Order> _orders = new();

    public Order Add(Drink drink, int quantity, TimeProvider clock)
    {
        ArgumentNullException.ThrowIfNull(drink);
        ArgumentNullException.ThrowIfNull(clock);

        var order = new Order(Guid.NewGuid(), drink.Id, drink.Name, quantity, drink.Price * quantity, clock.GetUtcNow());
        _orders[order.Id] = order;
        return order;
    }

    public Order? Find(Guid id) => _orders.GetValueOrDefault(id);
}
