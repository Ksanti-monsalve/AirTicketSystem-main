// src/modules/aircraft/Domain/ValueObjects/FechaProximoMantenimientoAircraft.cs
namespace AirTicketSystem.modules.aircraft.Domain.ValueObjects;

public sealed class FechaProximoMantenimientoAircraft
{
    public DateOnly Valor { get; }

    private FechaProximoMantenimientoAircraft(DateOnly valor) => Valor = valor;

    public static FechaProximoMantenimientoAircraft Crear(DateOnly valor)
    {
        // El próximo mantenimiento puede ser hoy o en el futuro
        var hoy = DateOnly.FromDateTime(DateTime.Today);

        if (valor < hoy)
            throw new ArgumentException(
                "La fecha del próximo mantenimiento no puede ser una fecha pasada.");

        // No se programa mantenimiento a más de 5 años
        if (valor > hoy.AddYears(5))
            throw new ArgumentException(
                "La fecha del próximo mantenimiento no puede superar 5 años en el futuro.");

        return new FechaProximoMantenimientoAircraft(valor);
    }

    public bool EsUrgente
    {
        get
        {
            var hoy = DateOnly.FromDateTime(DateTime.Today);
            return Valor.DayNumber - hoy.DayNumber <= 30;
        }
    }

    public int DiasRestantes
    {
        get
        {
            var hoy = DateOnly.FromDateTime(DateTime.Today);
            return Valor.DayNumber - hoy.DayNumber;
        }
    }

    public override string ToString() => Valor.ToString("dd/MM/yyyy");
}