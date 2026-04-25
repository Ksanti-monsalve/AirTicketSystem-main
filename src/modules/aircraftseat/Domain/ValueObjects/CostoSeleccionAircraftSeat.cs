// src/modules/aircraftseat/Domain/ValueObjects/CostoSeleccionAircraftSeat.cs
namespace AirTicketSystem.modules.aircraftseat.Domain.ValueObjects;

public sealed class CostoSeleccionAircraftSeat
{
    public decimal Valor { get; }

    private CostoSeleccionAircraftSeat(decimal valor) => Valor = valor;

    public static CostoSeleccionAircraftSeat Crear(decimal valor)
    {
        if (valor < 0)
            throw new ArgumentException(
                "El costo de selección de asiento no puede ser negativo.");

        if (valor > 999_999)
            throw new ArgumentException(
                "El costo de selección no puede superar 999.999.");

        return new CostoSeleccionAircraftSeat(Math.Round(valor, 2));
    }

    public static CostoSeleccionAircraftSeat Gratuito() => new(0);

    public bool EsGratuito => Valor == 0;

    public override string ToString() =>
        Valor == 0 ? "Gratuito" : $"{Valor:N2}";
}