// src/modules/flightcrew/Infrastructure/entity/FlightCrewEntity.cs
using AirTicketSystem.modules.flight.Infrastructure.entity;
using AirTicketSystem.modules.worker.Infrastructure.entity;

namespace AirTicketSystem.modules.flightcrew.Infrastructure.entity;

public class FlightCrewEntity
{
    public int Id { get; set; }
    public int VueloId { get; set; }
    public int TrabajadorId { get; set; }
    public string RolEnVuelo { get; set; } = null!;

    public FlightEntity Vuelo { get; set; } = null!;
    public WorkerEntity Trabajador { get; set; } = null!;
}