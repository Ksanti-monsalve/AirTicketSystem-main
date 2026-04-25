// src/modules/aircraft/Domain/ValueObjects/FechaUltimoMantenimientoAircraft.cs
namespace AirTicketSystem.modules.aircraft.Domain.ValueObjects;

public sealed class FechaUltimoMantenimientoAircraft
{
    public DateOnly Valor { get; }

    private FechaUltimoMantenimientoAircraft(DateOnly valor) => Valor = valor;

    public static FechaUltimoMantenimientoAircraft Crear(DateOnly valor)
    {
        var hoy = DateOnly.FromDateTime(DateTime.Today);

        if (valor > hoy)
            throw new ArgumentException(
                "La fecha del último mantenimiento no puede ser una fecha futura.");

        if (valor < new DateOnly(1940, 1, 1))
            throw new ArgumentException(
                "La fecha del último mantenimiento no puede ser anterior a 1940.");

        return new FechaUltimoMantenimientoAircraft(valor);
    }

    public int DiasDesdeUltimoMantenimiento
    {
        get
        {
            var hoy = DateOnly.FromDateTime(DateTime.Today);
            return hoy.DayNumber - Valor.DayNumber;
        }
    }

    public override string ToString() => Valor.ToString("dd/MM/yyyy");
}