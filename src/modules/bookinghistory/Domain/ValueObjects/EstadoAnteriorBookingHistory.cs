// src/modules/bookinghistory/Domain/ValueObjects/EstadoAnteriorBookingHistory.cs
namespace AirTicketSystem.modules.bookinghistory.Domain.ValueObjects;

public sealed class EstadoAnteriorBookingHistory
{
    private static readonly string[] _estadosValidos =
    {
        "PENDIENTE",
        "CONFIRMADA",
        "CANCELADA",
        "EXPIRADA"
    };

    public string Valor { get; }

    private EstadoAnteriorBookingHistory(string valor) => Valor = valor;

    public static EstadoAnteriorBookingHistory Crear(string? valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new ArgumentException("El estado anterior de la reserva no puede estar vacío.");

        var normalizado = valor.Trim().ToUpperInvariant();

        if (!_estadosValidos.Contains(normalizado))
            throw new ArgumentException(
                $"El estado anterior '{valor}' no es válido. " +
                $"Los valores permitidos son: {string.Join(", ", _estadosValidos)}");

        return new EstadoAnteriorBookingHistory(normalizado);
    }

    public override string ToString() => Valor;
}