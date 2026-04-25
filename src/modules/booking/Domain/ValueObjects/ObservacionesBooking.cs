// src/modules/booking/Domain/ValueObjects/ObservacionesBooking.cs
namespace AirTicketSystem.modules.booking.Domain.ValueObjects;

public sealed class ObservacionesBooking
{
    public string Valor { get; }

    private ObservacionesBooking(string valor) => Valor = valor;

    public static ObservacionesBooking Crear(string? valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new ArgumentException("Las observaciones no pueden estar vacías.");

        var normalizado = valor.Trim();

        if (normalizado.Length < 3)
            throw new ArgumentException(
                "Las observaciones deben tener al menos 3 caracteres.");

        if (normalizado.Length > 300)
            throw new ArgumentException(
                "Las observaciones no pueden superar 300 caracteres.");

        return new ObservacionesBooking(normalizado);
    }

    public override string ToString() => Valor;
}