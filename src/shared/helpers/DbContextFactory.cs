// src/shared/helpers/DbContextFactory.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using AirTicketSystem.shared.context;

namespace AirTicketSystem.shared.helpers;

/// <summary>
/// Fábrica de AppDbContext para uso en tiempo de ejecución (Program.cs).
/// Separa la creación del contexto de la inyección de dependencias.
/// </summary>
public static class DbContextFactory
{
    public static AppDbContext Create(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException(
                "No se encontró 'DefaultConnection' en appsettings.json");

        var serverVersion = MySqlVersionResolver.Resolve(connectionString);

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseMySql(connectionString, serverVersion, mySqlOptions =>
            {
                mySqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 3,
                    maxRetryDelay: TimeSpan.FromSeconds(5),
                    errorNumbersToAdd: null);
            })
            .Options;

        return new AppDbContext(options);
    }
}
