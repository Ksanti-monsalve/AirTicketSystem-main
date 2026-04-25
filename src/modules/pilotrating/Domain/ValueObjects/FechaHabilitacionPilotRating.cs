// src/modules/pilotrating/Domain/ValueObjects/FechaHabilitacionPilotRating.cs
namespace AirTicketSystem.modules.pilotrating.Domain.ValueObjects;

public sealed class FechaHabilitacionPilotRating
{
    public DateOnly Valor { get; }

    private FechaHabilitacionPilotRating(DateOnly valor) => Valor = valor;

    public static FechaHabilitacionPilotRating Crear(DateOnly valor)
    {
        var hoy = DateOnly.FromDateTime(DateTime.Today);

        if (valor > hoy)
            throw new ArgumentException(
                "La fecha de habilitación no puede ser una fecha futura.");

        if (valor < new DateOnly(1950, 1, 1))
            throw new ArgumentException(
                "La fecha de habilitación no puede ser anterior a 1950.");

        return new FechaHabilitacionPilotRating(valor);
    }

    public override string ToString() => Valor.ToString("dd/MM/yyyy");
}