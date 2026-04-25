// src/modules/flighthistory/Domain/ValueObjects/FechaCambioFlightHistory.cs
namespace AirTicketSystem.modules.flighthistory.Domain.ValueObjects;

public sealed class FechaCambioFlightHistory
{
    public DateTime Valor { get; }

    private FechaCambioFlightHistory(DateTime valor) => Valor = valor;

    public static FechaCambioFlightHistory Crear(DateTime valor)
    {
        if (valor == default)
            throw new ArgumentException("La fecha de cambio no puede estar vacía.");

        if (valor > DateTime.UtcNow.AddMinutes(1))
            throw new ArgumentException(
                "La fecha de cambio no puede ser una fecha futura.");

        if (valor < new DateTime(2000, 1, 1))
            throw new ArgumentException(
                "La fecha de cambio no puede ser anterior al año 2000.");

        return new FechaCambioFlightHistory(valor);
    }

    public static FechaCambioFlightHistory Ahora() => new(DateTime.UtcNow);

    public override string ToString() => Valor.ToString("dd/MM/yyyy HH:mm:ss");
}