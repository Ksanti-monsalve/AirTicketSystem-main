// src/modules/flightcrew/Domain/aggregate/FlightCrew.cs
using AirTicketSystem.modules.flightcrew.Domain.ValueObjects;

namespace AirTicketSystem.modules.flightcrew.Domain.aggregate;

public sealed class FlightCrew
{
    public int Id { get; private set; }
    public int VueloId { get; private set; }
    public int TrabajadorId { get; private set; }
    public RolEnVueloFlightCrew RolEnVuelo { get; private set; } = null!;

    private FlightCrew() { }

    public static FlightCrew Crear(int vueloId, int trabajadorId, string rolEnVuelo)
    {
        if (vueloId <= 0)
            throw new ArgumentException("El vuelo es obligatorio.");

        if (trabajadorId <= 0)
            throw new ArgumentException("El trabajador es obligatorio.");

        return new FlightCrew
        {
            VueloId      = vueloId,
            TrabajadorId = trabajadorId,
            RolEnVuelo   = RolEnVueloFlightCrew.Crear(rolEnVuelo)
        };
    }

    public static FlightCrew Reconstituir(
        int id, int vueloId, int trabajadorId, string rolEnVuelo)
    {
        return new FlightCrew
        {
            Id           = id,
            VueloId      = vueloId,
            TrabajadorId = trabajadorId,
            RolEnVuelo   = RolEnVueloFlightCrew.Crear(rolEnVuelo)
        };
    }

    public void EstablecerId(int id)
    {
        if (Id != 0)
            throw new InvalidOperationException("El ID ya fue establecido.");

        if (id <= 0)
            throw new ArgumentException("El ID debe ser mayor a 0.");

        Id = id;
    }

    public bool EsParteDeCabina =>
        RolEnVuelo.Valor == "PILOTO" || RolEnVuelo.Valor == "COPILOTO";

    public override string ToString()
        => $"[{RolEnVuelo}] Trabajador #{TrabajadorId} — Vuelo #{VueloId}";
}