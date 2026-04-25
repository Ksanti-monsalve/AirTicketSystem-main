// src/shared/context/AppDbContextDesignTimeFactory.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using AirTicketSystem.shared.helpers;

namespace AirTicketSystem.shared.context;

/// <summary>
/// Esta clase es usada EXCLUSIVAMENTE por las herramientas de EF Core
/// en tiempo de diseño (dotnet ef migrations add, dotnet ef database update).
/// 
/// ¿Por qué existe? Cuando corrés un comando de migración, EF Core necesita
/// construir el DbContext sin ejecutar Program.cs. Esta factory le enseña
/// cómo hacerlo leyendo el appsettings.json directamente.
/// </summary>
public class AppDbContextDesignTimeFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        // Construye la configuración leyendo appsettings.json desde la raíz del proyecto
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException(
                "No se encontró 'DefaultConnection' en appsettings.json");

        var serverVersion = MySqlVersionResolver.Resolve(connectionString);

        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        optionsBuilder.UseMySql(connectionString, serverVersion);

        return new AppDbContext(optionsBuilder.Options);
    }
}
