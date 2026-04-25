// src/shared/helpers/ConsoleErrorHandler.cs
namespace AirTicketSystem.shared.helpers;

/// <summary>
/// Centraliza el manejo de errores en la capa de UI.
/// Convierte excepciones técnicas en mensajes amigables para el usuario.
/// El programa NUNCA debe romparse — siempre muestra un mensaje y continúa.
/// </summary>
public static class ConsoleErrorHandler
{
    /// <summary>
    /// Ejecuta una acción y atrapa cualquier excepción mostrando
    /// un mensaje amigable. El programa continúa ejecutándose.
    /// </summary>
    public static async Task ExecuteAsync(Func<Task> accion)
    {
        try
        {
            await accion();
        }
        catch (ArgumentException ex)
        {
            MostrarError("Dato inválido", ex.Message);
        }
        catch (KeyNotFoundException ex)
        {
            MostrarError("No encontrado", ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            MostrarError("Operación no permitida", ex.Message);
        }
        catch (Microsoft.EntityFrameworkCore.DbUpdateException ex)
        {
            // Error de base de datos — puede ser FK violation, unique constraint, etc.
            var mensaje = ObtenerMensajeDbError(ex);
            MostrarError("Error de base de datos", mensaje);
        }
        catch (Exception ex)
        {
            // Cualquier otro error inesperado
            MostrarError("Error inesperado", ex.Message);
        }
    }

    /// <summary>
    /// Versión que retorna un valor. Si hay error, retorna el valor por defecto.
    /// </summary>
    public static async Task<T?> ExecuteAsync<T>(Func<Task<T>> accion)
    {
        try
        {
            return await accion();
        }
        catch (ArgumentException ex)
        {
            MostrarError("Dato inválido", ex.Message);
            return default;
        }
        catch (KeyNotFoundException ex)
        {
            MostrarError("No encontrado", ex.Message);
            return default;
        }
        catch (InvalidOperationException ex)
        {
            MostrarError("Operación no permitida", ex.Message);
            return default;
        }
        catch (Microsoft.EntityFrameworkCore.DbUpdateException ex)
        {
            var mensaje = ObtenerMensajeDbError(ex);
            MostrarError("Error de base de datos", mensaje);
            return default;
        }
        catch (Exception ex)
        {
            MostrarError("Error inesperado", ex.Message);
            return default;
        }
    }

    private static void MostrarError(string tipo, string mensaje)
    {
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"  ✗ {tipo}: {mensaje}");
        Console.ResetColor();
        Console.WriteLine();
    }

    private static string ObtenerMensajeDbError(
        Microsoft.EntityFrameworkCore.DbUpdateException ex)
    {
        var inner = ex.InnerException?.Message ?? ex.Message;

        // MySQL error codes comunes
        if (inner.Contains("Duplicate entry") || inner.Contains("unique"))
            return "Ya existe un registro con esos datos. " +
                   "Verifique que no haya duplicados.";

        if (inner.Contains("foreign key") || inner.Contains("FOREIGN KEY"))
            return "No se puede realizar la operación porque el registro " +
                   "está relacionado con otros datos del sistema.";

        if (inner.Contains("cannot be null") || inner.Contains("NOT NULL"))
            return "Hay campos obligatorios sin completar.";

        return "Ocurrió un error al guardar en la base de datos. " +
               "Intente nuevamente.";
    }
}