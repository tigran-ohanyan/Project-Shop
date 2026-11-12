using Microsoft.EntityFrameworkCore;

namespace Shop;

public class Order
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public int Total { get; set; }
    public List<OrderLine> Lines { get; set; } = new();
}

public class OrderLine
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public int Price { get; set; }
}

public class ShopDb(DbContextOptions<ShopDb> options) : DbContext(options)
{
    public DbSet<Order> Orders => Set<Order>();
}