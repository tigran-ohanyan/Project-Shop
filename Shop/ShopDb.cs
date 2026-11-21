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
    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder b) =>
        b.Entity<Product>().HasData(
            new Product { Id = 1, Name = "Меч", Price = 500 },
            new Product { Id = 2, Name = "Щит", Price = 300 },
            new Product { Id = 3, Name = "Зелье", Price = 50 });
}

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public int Price { get; set; }
}