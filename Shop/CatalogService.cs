namespace Shop;

public record Item(string Name, int Price);

public class CatalogService(ShopDb db)

{
    public List<Item> Items { get; } = new()
    {
        new("Меч", 500),
        new("Щит", 300),
        new("Зелье", 50)
    };

    public List<Item> Cart { get; } = new();

    public void Buy(Item item) => Cart.Add(item);

    public void Remove(Item item) => Cart.Remove(item);

    public int Total => Cart.Sum(i => i.Price);

    public async Task Checkout()
    {
        var order = new Order
        {
            CreatedAt = DateTime.Now,
            Total = Total,
            Lines = Cart.Select(i => new OrderLine { Name = i.Name, Price = i.Price }).ToList()
        };
        db.Orders.Add(order);
        await db.SaveChangesAsync();
        Cart.Clear();
    }
}