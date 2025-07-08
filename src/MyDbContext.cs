using ContainerizedAPI.Entities;
using Microsoft.EntityFrameworkCore;

namespace ContainerizedAPI;

public class MyDbContext : DbContext
{
    public MyDbContext(DbContextOptions options) : base(options)
    {

    }
    public DbSet<Product> Products { get; set; }
}
