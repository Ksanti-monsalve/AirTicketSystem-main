// src/modules/aircraftseat/Domain/ValueObjects/EsVentanaAircraftSeat.cs
namespace AirTicketSystem.modules.aircraftseat.Domain.ValueObjects;

public sealed class EsVentanaAircraftSeat
{
    public bool Valor { get; }

    private EsVentanaAircraftSeat(bool valor) => Valor = valor;

    public static EsVentanaAircraftSeat Crear(bool valor) => new(valor);

    public static EsVentanaAircraftSeat Ventana() => new(true);

    public static EsVentanaAircraftSeat NoVentana() => new(false);

    public override string ToString() => Valor ? "Ventana" : "No es ventana";
}