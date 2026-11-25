using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ZooLogic.Api.Data;

/// <summary>
/// Permite generar migraciones sin arrancar la API ni disponer de una base de datos.
/// </summary>
public sealed class ZooDbContextFactory : IDesignTimeDbContextFactory<ZooDbContext>
{
    public ZooDbContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<ZooDbContext>()
            .UseNpgsql()
            .Options;

        return new ZooDbContext(options);
    }
}
