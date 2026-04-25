// src/modules/aircraft/Infrastructure/entity/AircraftEntity.cs
using AirTicketSystem.modules.aircraftmodel.Infrastructure.entity;
using AirTicketSystem.modules.aircraftseat.Infrastructure.entity;
using AirTicketSystem.modules.airline.Infrastructure.entity;
using AirTicketSystem.modules.flight.Infrastructure.entity;

namespace AirTicketSystem.modules.aircraft.Infrastructure.entity;

public class AircraftEntity
{
    public int Id { get; set; }
    public string Matricula { get; set; } = null!;
    public int ModeloAvionId { get; set; }
    public int AerolineaId { get; set; }
    public DateOnly? FechaFabricacion { get; set; }
    public DateOnly? FechaUltimoMantenimiento { get; set; }
    public DateOnly? FechaProximoMantenimiento { get; set; }
    public decimal TotalHorasVuelo { get; set; } = 0;
    public string Estado { get; set; } = "DISPONIBLE";
    public bool Activo { get; set; } = true;

    public AircraftModelEntity ModeloAvion { get; set; } = null!;
    public AirlineEntity Aerolinea { get; set; } = null!;
    public ICollection<AircraftSeatEntity> Asientos { get; set; } = new List<AircraftSeatEntity>();
    public ICollection<FlightEntity> Vuelos { get; set; } = new List<FlightEntity>();
}