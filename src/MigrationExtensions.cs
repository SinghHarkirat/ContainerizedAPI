using Microsoft.EntityFrameworkCore;

namespace ContainerizedAPI.src;

public static class MigrationExtensions
{
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using IServiceScope serviceScope = app.ApplicationServices.CreateScope();
        using MyDbContext myDbContext = serviceScope.ServiceProvider.GetService<MyDbContext>();
        myDbContext.Database.Migrate();
    }
}
