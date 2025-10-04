using Microsoft.EntityFrameworkCore;

namespace Myshop.Models;

public class ItemDbContext : DbContext
{
    public ItemDbContext(DbContextOptions<ItemDbContext> options) : base(options)
    {
        Database.EnsureCreated();
    }

    public DbSet<Item> Items { get; set; }
}