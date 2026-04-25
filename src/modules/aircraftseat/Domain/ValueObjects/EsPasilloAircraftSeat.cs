// src/modules/aircraftseat/Domain/ValueObjects/EsPasilloAircraftSeat.cs
namespace AirTicketSystem.modules.aircraftseat.Domain.ValueObjects;

public sealed class EsPasilloAircraftSeat
{
    public bool Valor { get; }

    private EsPasilloAircraftSeat(bool valor) => Valor = valor;

    public static EsPasilloAircraftSeat Crear(bool valor) => new(valor);

    public static EsPasilloAircraftSeat Pasillo() => new(true);

    public static EsPasilloAircraftSeat NoPasillo() => new(false);

    public override string ToString() => Valor ? "Pasillo" : "No es pasillo";
}