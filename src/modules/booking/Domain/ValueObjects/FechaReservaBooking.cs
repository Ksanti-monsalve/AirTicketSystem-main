// src/modules/booking/Domain/ValueObjects/FechaReservaBooking.cs
namespace AirTicketSystem.modules.booking.Domain.ValueObjects;

public sealed class FechaReservaBooking
{
    public DateTime Valor { get; }

    private FechaReservaBooking(DateTime valor) => Valor = valor;

    public static FechaReservaBooking Crear(DateTime valor)
    {
        if (valor == default)
            throw new ArgumentException("La fecha de reserva no puede estar vacía.");

        if (valor > DateTime.UtcNow.AddMinutes(1))
            throw new ArgumentException(
                "La fecha de reserva no puede ser una fecha futura.");

        if (valor < new DateTime(2000, 1, 1))
            throw new ArgumentException(
                "La fecha de reserva no puede ser anterior al año 2000.");

        return new FechaReservaBooking(valor);
    }

    public static FechaReservaBooking Ahora() => new(DateTime.UtcNow);

    public string EnFormatoCorto => Valor.ToString("dd/MM/yyyy HH:mm");

    public override string ToString() => Valor.ToString("dd/MM/yyyy HH:mm:ss");
}