// src/modules/aircraft/Domain/ValueObjects/FechaFabricacionAircraft.cs
namespace AirTicketSystem.modules.aircraft.Domain.ValueObjects;

public sealed class FechaFabricacionAircraft
{
    public DateOnly Valor { get; }

    private FechaFabricacionAircraft(DateOnly valor) => Valor = valor;

    public static FechaFabricacionAircraft Crear(DateOnly valor)
    {
        var hoy = DateOnly.FromDateTime(DateTime.Today);

        if (valor > hoy)
            throw new ArgumentException("La fecha de fabricación no puede ser una fecha futura.");

        // Primer avión comercial moderno: años 40
        if (valor < new DateOnly(1940, 1, 1))
            throw new ArgumentException(
                "La fecha de fabricación no puede ser anterior a 1940.");

        return new FechaFabricacionAircraft(valor);
    }

    public int AnosEnServicio
    {
        get
        {
            var hoy = DateOnly.FromDateTime(DateTime.Today);
            return hoy.Year - Valor.Year;
        }
    }

    public override string ToString() => Valor.ToString("dd/MM/yyyy");
}