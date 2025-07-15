using ContainerizedAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace ContainerizedAPI;

public class MyDbContext : DbContext
{
    public MyDbContext(DbContextOptions options) : base(options)
    {

    }
    override protected void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Product>().HasIndex(i => i.Name).IsUnique();
        modelBuilder.Entity<Product>().Property(p => p.Name)
            .IsRequired().HasMaxLength(400);
    }
    public DbSet<Product> Products { get; set; }
}
