// src/modules/aircraft/Domain/ValueObjects/TotalHorasVueloAircraft.cs
namespace AirTicketSystem.modules.aircraft.Domain.ValueObjects;

public sealed class TotalHorasVueloAircraft
{
    public decimal Valor { get; }

    private TotalHorasVueloAircraft(decimal valor) => Valor = valor;

    public static TotalHorasVueloAircraft Crear(decimal valor)
    {
        if (valor < 0)
            throw new ArgumentException("El total de horas de vuelo no puede ser negativo.");

        // Aviones pueden acumular hasta ~100.000 horas en su vida útil
        if (valor > 100_000)
            throw new ArgumentException(
                "El total de horas de vuelo no puede superar 100.000 horas.");

        return new TotalHorasVueloAircraft(Math.Round(valor, 2));
    }

    public static TotalHorasVueloAircraft Cero() => new(0);

    public TotalHorasVueloAircraft Sumar(decimal horas)
    {
        if (horas <= 0)
            throw new ArgumentException("Las horas a sumar deben ser mayores a 0.");

        return Crear(Valor + horas);
    }

    public override string ToString() => $"{Valor:N2} horas";
}