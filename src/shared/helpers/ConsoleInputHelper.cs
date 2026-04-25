// src/shared/helpers/ConsoleInputHelper.cs
namespace AirTicketSystem.shared.helpers;

/// <summary>
/// Centraliza la lectura de inputs del usuario desde consola.
/// Nunca lanza excepción — siempre valida y repite si el dato es inválido.
/// </summary>
public static class ConsoleInputHelper
{
    /// <summary>
    /// Lee un texto obligatorio. No acepta vacío.
    /// </summary>
    public static string LeerTextoObligatorio(string etiqueta)
    {
        while (true)
        {
            Console.Write($"  {etiqueta}: ");
            var input = Console.ReadLine()?.Trim();

            if (!string.IsNullOrWhiteSpace(input))
                return input;

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"  ⚠ El campo '{etiqueta}' es obligatorio.");
            Console.ResetColor();
        }
    }

    /// <summary>
    /// Lee un texto opcional. Puede estar vacío.
    /// </summary>
    public static string? LeerTextoOpcional(string etiqueta)
    {
        Console.Write($"  {etiqueta} (opcional, Enter para omitir): ");
        var input = Console.ReadLine()?.Trim();
        return string.IsNullOrWhiteSpace(input) ? null : input;
    }

    /// <summary>
    /// Lee un número entero. Repite hasta que sea válido.
    /// </summary>
    public static int LeerEntero(string etiqueta)
    {
        while (true)
        {
            Console.Write($"  {etiqueta}: ");
            var input = Console.ReadLine()?.Trim();

            if (int.TryParse(input, out var numero))
                return numero;

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"  ⚠ Debe ingresar un número entero válido.");
            Console.ResetColor();
        }
    }

    /// <summary>
    /// Lee un número entero dentro de un rango.
    /// </summary>
    public static int LeerEnteroEnRango(string etiqueta, int min, int max)
    {
        while (true)
        {
            var numero = LeerEntero(etiqueta);

            if (numero >= min && numero <= max)
                return numero;

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(
                $"  ⚠ El valor debe estar entre {min} y {max}.");
            Console.ResetColor();
        }
    }

    /// <summary>
    /// Lee un número decimal. Repite hasta que sea válido.
    /// </summary>
    public static decimal LeerDecimal(string etiqueta)
    {
        while (true)
        {
            Console.Write($"  {etiqueta}: ");
            var input = Console.ReadLine()?.Trim()?.Replace(",", ".");

            if (decimal.TryParse(input,
                System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.InvariantCulture,
                out var numero))
                return numero;

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"  ⚠ Debe ingresar un número decimal válido.");
            Console.ResetColor();
        }
    }

    /// <summary>
    /// Lee una fecha en formato dd/MM/yyyy.
    /// </summary>
    public static DateOnly LeerFecha(string etiqueta)
    {
        while (true)
        {
            Console.Write($"  {etiqueta} (dd/MM/yyyy): ");
            var input = Console.ReadLine()?.Trim();

            if (DateOnly.TryParseExact(input, "dd/MM/yyyy",
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None,
                out var fecha))
                return fecha;

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(
                $"  ⚠ Formato de fecha inválido. Use dd/MM/yyyy " +
                $"(ejemplo: 25/12/2025).");
            Console.ResetColor();
        }
    }

    /// <summary>
    /// Lee una fecha y hora en formato dd/MM/yyyy HH:mm.
    /// </summary>
    public static DateTime LeerFechaHora(string etiqueta)
    {
        while (true)
        {
            Console.Write($"  {etiqueta} (dd/MM/yyyy HH:mm): ");
            var input = Console.ReadLine()?.Trim();

            if (DateTime.TryParseExact(input, "dd/MM/yyyy HH:mm",
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None,
                out var fechaHora))
                return fechaHora;

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(
                $"  ⚠ Formato inválido. Use dd/MM/yyyy HH:mm " +
                $"(ejemplo: 25/12/2025 14:30).");
            Console.ResetColor();
        }
    }

    /// <summary>
    /// Lee una opción de menú y la valida contra las opciones disponibles.
    /// </summary>
    public static int LeerOpcionMenu(int min, int max)
    {
        return LeerEnteroEnRango("Seleccione una opción", min, max);
    }

    /// <summary>
    /// Pide confirmación al usuario (S/N).
    /// </summary>
    public static bool Confirmar(string mensaje)
    {
        while (true)
        {
            Console.Write($"  {mensaje} (S/N): ");
            var input = Console.ReadLine()?.Trim().ToUpperInvariant();

            if (input == "S") return true;
            if (input == "N") return false;

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("  ⚠ Ingrese S para sí o N para no.");
            Console.ResetColor();
        }
    }

    /// <summary>
    /// Pausa la ejecución hasta que el usuario presione Enter.
    /// </summary>
    public static void EsperarTecla(string mensaje = "Presione Enter para continuar...")
    {
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.Write($"  {mensaje}");
        Console.ResetColor();
        Console.ReadLine();
    }
}