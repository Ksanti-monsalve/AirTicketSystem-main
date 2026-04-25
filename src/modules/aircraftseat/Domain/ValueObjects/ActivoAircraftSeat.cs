// src/modules/aircraftseat/Domain/ValueObjects/ActivoAircraftSeat.cs
namespace AirTicketSystem.modules.aircraftseat.Domain.ValueObjects;

public sealed class ActivoAircraftSeat
{
    public bool Valor { get; }

    private ActivoAircraftSeat(bool valor) => Valor = valor;

    public static ActivoAircraftSeat Crear(bool valor) => new(valor);

    public static ActivoAircraftSeat Activo() => new(true);

    public static ActivoAircraftSeat Inactivo() => new(false);

    public override string ToString() => Valor ? "Activo" : "Inactivo";
}
