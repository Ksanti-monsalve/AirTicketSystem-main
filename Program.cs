// Program.cs
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using AirTicketSystem.shared;
using AirTicketSystem.shared.UI;
using AirTicketSystem.modules.user.Application.UseCases;
using AirTicketSystem.UI.Auth;
using AirTicketSystem.UI;

// ── Configuración ────────────────────────────────────────────────────────────
var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

// ── Contenedor DI ────────────────────────────────────────────────────────────
var services = new ServiceCollection();
services.AddSingleton<IConfiguration>(configuration);
services.AddAirTicketSystem(configuration);

var provider = services.BuildServiceProvider();

// ── Verificar conexión y aplicar migraciones pendientes ──────────────────────
try
{
    await using var startScope = provider.CreateAsyncScope();
    var context = startScope.ServiceProvider
        .GetRequiredService<AirTicketSystem.shared.context.AppDbContext>();
    await context.Database.CanConnectAsync();

    // Aplica automáticamente cualquier migración pendiente
    var pendientes = (await context.Database.GetPendingMigrationsAsync()).ToList();
    if (pendientes.Count > 0)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"  ► Aplicando {pendientes.Count} migración(es) pendiente(s)...");
        await context.Database.MigrateAsync();
        Console.WriteLine("  ✓ Migraciones aplicadas.");
        Console.ResetColor();
    }
}
catch (Exception ex)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine($"  ✗ Error de base de datos: {ex.Message}");
    Console.ResetColor();
    Console.WriteLine("  Presione cualquier tecla para salir...");
    Console.ReadKey(true);
    return;
}

// ── Seed de usuario administrador ────────────────────────────────────────────
try
{
    await using var seedScope = provider.CreateAsyncScope();
    var seed = seedScope.ServiceProvider.GetRequiredService<SeedAdminUseCase>();
    await seed.ExecuteAsync();
}
catch (Exception ex)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine($"  ✗ Error en seed admin: {ex.Message}");
    Console.ResetColor();
    Console.WriteLine("  Presione cualquier tecla para continuar...");
    Console.ReadKey(true);
}

// ── Seed completo de datos de prueba ─────────────────────────────────────────
try
{
    var seedFull = provider.GetRequiredService<AirTicketSystem.shared.SeedFullDataUseCase>();
    await seedFull.ExecuteAsync();
}
catch (Exception ex)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine();
    Console.WriteLine("  ✗ ERROR en seed de datos de prueba:");
    Console.WriteLine($"    {ex.GetType().Name}: {ex.Message}");
    if (ex.InnerException is not null)
        Console.WriteLine($"    Inner: {ex.InnerException.Message}");
    Console.WriteLine();
    Console.WriteLine("  Stack trace:");
    Console.WriteLine(ex.StackTrace);
    Console.ResetColor();
    Console.WriteLine();
    Console.WriteLine("  Presione cualquier tecla para continuar (el sistema funcionará");
    Console.WriteLine("  pero sin datos de prueba precargados)...");
    Console.ReadKey(true);
}

// ── Bucle principal ──────────────────────────────────────────────────────────
var session = provider.GetRequiredService<SessionContext>();

while (true)
{
    var authMenu = new AuthMenu(provider, session);
    var autenticado = await authMenu.MostrarAsync();

    if (!autenticado)
    {
        Spectre.Console.AnsiConsole.MarkupLine(
            "\n[grey]  Hasta luego. ¡Buen vuelo![/]\n");
        break;
    }

    var mainMenu = new MainMenu(provider, session);
    await mainMenu.EnrutarAsync();
}
