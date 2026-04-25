// src/modules/bookinghistory/Domain/ValueObjects/MotivoBookingHistory.cs
namespace AirTicketSystem.modules.bookinghistory.Domain.ValueObjects;

public sealed class MotivoBookingHistory
{
    public string Valor { get; }

    private MotivoBookingHistory(string valor) => Valor = valor;

    public static MotivoBookingHistory Crear(string? valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new ArgumentException("El motivo del cambio no puede estar vacío.");

        var normalizado = valor.Trim();

        if (normalizado.Length < 5)
            throw new ArgumentException(
                "El motivo debe tener al menos 5 caracteres para ser descriptivo.");

        if (normalizado.Length > 300)
            throw new ArgumentException(
                "El motivo no puede superar 300 caracteres.");

        return new MotivoBookingHistory(normalizado);
    }

    public override string ToString() => Valor;
}