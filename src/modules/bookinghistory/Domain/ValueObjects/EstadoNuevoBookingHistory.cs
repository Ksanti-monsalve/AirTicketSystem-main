// src/modules/bookinghistory/Domain/ValueObjects/EstadoNuevoBookingHistory.cs
namespace AirTicketSystem.modules.bookinghistory.Domain.ValueObjects;

public sealed class EstadoNuevoBookingHistory
{
    private static readonly string[] _estadosValidos =
    {
        "PENDIENTE",
        "CONFIRMADA",
        "CANCELADA",
        "EXPIRADA"
    };

    public string Valor { get; }

    private EstadoNuevoBookingHistory(string valor) => Valor = valor;

    public static EstadoNuevoBookingHistory Crear(string? valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new ArgumentException("El estado nuevo de la reserva no puede estar vacío.");

        var normalizado = valor.Trim().ToUpperInvariant();

        if (!_estadosValidos.Contains(normalizado))
            throw new ArgumentException(
                $"El estado nuevo '{valor}' no es válido. " +
                $"Los valores permitidos son: {string.Join(", ", _estadosValidos)}");

        return new EstadoNuevoBookingHistory(normalizado);
    }

    public override string ToString() => Valor;
}