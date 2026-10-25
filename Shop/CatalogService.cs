namespace Shop;

public record Item(string Name, int Price);

public class CatalogService
{
    public List<Item> Items { get; } = new()
    {
        new("Меч", 500),
        new("Щит", 300),
        new("Зелье", 50)
    };

    public List<Item> Cart { get; } = new();

    public void Buy(Item item) => Cart.Add(item);
}