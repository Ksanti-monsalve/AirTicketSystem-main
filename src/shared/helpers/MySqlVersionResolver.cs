// src/shared/helpers/MySqlVersionResolver.cs
using Microsoft.EntityFrameworkCore;

namespace AirTicketSystem.shared.helpers;

/// <summary>
/// Resuelve la versión de MySQL en tiempo de ejecución para que EF Core
/// genere SQL compatible. Si falla la detección automática, usa 8.0.
/// </summary>
public static class MySqlVersionResolver
{
    public static ServerVersion Resolve(string connectionString)
    {
        try
        {
            return ServerVersion.AutoDetect(connectionString);
        }
        catch
        {
            // Fallback seguro a MySQL 8.0 si el servidor no responde aún
            return new MySqlServerVersion(new Version(8, 0, 0));
        }
    }
}
