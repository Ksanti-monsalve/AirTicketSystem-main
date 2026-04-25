// src/modules/airport/Domain/ValueObjects/ActivoAirport.cs
namespace AirTicketSystem.modules.airport.Domain.ValueObjects;

public sealed class ActivoAirport
{
    public bool Valor { get; }

    private ActivoAirport(bool valor) => Valor = valor;

    public static ActivoAirport Crear(bool valor) => new(valor);

    public static ActivoAirport Activo() => new(true);

    public static ActivoAirport Inactivo() => new(false);

    public override string ToString() => Valor ? "Activo" : "Inactivo";
}