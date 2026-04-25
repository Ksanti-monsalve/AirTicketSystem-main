// src/modules/aircraft/Domain/ValueObjects/ActivoAircraft.cs
namespace AirTicketSystem.modules.aircraft.Domain.ValueObjects;

public sealed class ActivoAircraft
{
    public bool Valor { get; }

    private ActivoAircraft(bool valor) => Valor = valor;

    public static ActivoAircraft Crear(bool valor) => new(valor);

    public static ActivoAircraft Activo() => new(true);

    public static ActivoAircraft Inactivo() => new(false);

    public override string ToString() => Valor ? "Activo" : "Inactivo";
}