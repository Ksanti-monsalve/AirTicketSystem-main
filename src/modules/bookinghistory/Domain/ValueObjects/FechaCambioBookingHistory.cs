// src/modules/bookinghistory/Domain/ValueObjects/FechaCambioBookingHistory.cs
namespace AirTicketSystem.modules.bookinghistory.Domain.ValueObjects;

public sealed class FechaCambioBookingHistory
{
    public DateTime Valor { get; }

    private FechaCambioBookingHistory(DateTime valor) => Valor = valor;

    public static FechaCambioBookingHistory Crear(DateTime valor)
    {
        if (valor == default)
            throw new ArgumentException("La fecha de cambio no puede estar vacía.");

        if (valor > DateTime.UtcNow.AddMinutes(1))
            throw new ArgumentException(
                "La fecha de cambio no puede ser una fecha futura.");

        if (valor < new DateTime(2000, 1, 1))
            throw new ArgumentException(
                "La fecha de cambio no puede ser anterior al año 2000.");

        return new FechaCambioBookingHistory(valor);
    }

    public static FechaCambioBookingHistory Ahora() => new(DateTime.UtcNow);

    public override string ToString() => Valor.ToString("dd/MM/yyyy HH:mm:ss");
}