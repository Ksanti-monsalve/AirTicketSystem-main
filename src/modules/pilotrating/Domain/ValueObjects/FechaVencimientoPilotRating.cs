// src/modules/pilotrating/Domain/ValueObjects/FechaVencimientoPilotRating.cs
namespace AirTicketSystem.modules.pilotrating.Domain.ValueObjects;

public sealed class FechaVencimientoPilotRating
{
    public DateOnly Valor { get; }

    private FechaVencimientoPilotRating(DateOnly valor) => Valor = valor;

    public static FechaVencimientoPilotRating Crear(DateOnly valor)
    {
        if (valor < new DateOnly(2000, 1, 1))
            throw new ArgumentException(
                "La fecha de vencimiento de la habilitación no puede ser anterior al año 2000.");

        var hoy = DateOnly.FromDateTime(DateTime.Today);

        if (valor > hoy.AddYears(5))
            throw new ArgumentException(
                "La fecha de vencimiento de la habilitación no puede superar 5 años en el futuro.");

        return new FechaVencimientoPilotRating(valor);
    }

    public bool EstaVigente
    {
        get
        {
            var hoy = DateOnly.FromDateTime(DateTime.Today);
            return Valor >= hoy;
        }
    }

    public bool VenceProximamente
    {
        get
        {
            var hoy = DateOnly.FromDateTime(DateTime.Today);
            var diasRestantes = Valor.DayNumber - hoy.DayNumber;
            return diasRestantes >= 0 && diasRestantes <= 60;
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