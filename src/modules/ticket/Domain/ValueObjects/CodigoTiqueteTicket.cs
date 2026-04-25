// src/modules/ticket/Domain/ValueObjects/CodigoTiqueteTicket.cs
namespace AirTicketSystem.modules.ticket.Domain.ValueObjects;

public sealed class CodigoTiqueteTicket
{
    public string Valor { get; }

    private CodigoTiqueteTicket(string valor) => Valor = valor;

    public static CodigoTiqueteTicket Crear(string? valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new ArgumentException("El código del tiquete no puede estar vacío.");

        var normalizado = valor.Trim().ToUpperInvariant();

        if (!normalizado.StartsWith("TK"))
            throw new ArgumentException(
                $"El código del tiquete debe comenzar con 'TK'. Se recibió: '{valor}'");

        var digitosParte = normalizado[2..];

        if (digitosParte.Length != 10)
            throw new ArgumentException(
                $"El código del tiquete debe tener el formato TK + 10 dígitos. Se recibió: '{valor}'");

        if (!digitosParte.All(char.IsDigit))
            throw new ArgumentException(
                $"Los caracteres después de 'TK' deben ser dígitos. Se recibió: '{valor}'");

        return new CodigoTiqueteTicket(normalizado);
    }

    /// <summary>
    /// Genera un código único basado en timestamp + componente aleatorio.
    /// Formato: TK + 10 dígitos.
    /// El componente aleatorio evita colisiones en generaciones simultáneas.
    /// </summary>
    public static CodigoTiqueteTicket Generar()
    {
        var timestamp = DateTime.UtcNow.Ticks.ToString();
        var ultimosSeis = timestamp[^6..];
        var aleatorio = new Random().Next(1000, 9999).ToString();
        var digitos = $"{ultimosSeis}{aleatorio}";
        return new CodigoTiqueteTicket($"TK{digitos}");
    }

    public override string ToString() => Valor;
}