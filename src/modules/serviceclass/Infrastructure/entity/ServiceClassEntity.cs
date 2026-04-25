// src/modules/serviceclass/Infrastructure/entity/ServiceClassEntity.cs
using AirTicketSystem.modules.aircraftseat.Infrastructure.entity;
using AirTicketSystem.modules.fare.Infrastructure.entity;

namespace AirTicketSystem.modules.serviceclass.Infrastructure.entity;

public class ServiceClassEntity
{
    public int Id { get; set; }
    public string Nombre { get; set; } = null!;
    public string Codigo { get; set; } = null!;
    public string? Descripcion { get; set; }

    public ICollection<AircraftSeatEntity> Asientos { get; set; } = new List<AircraftSeatEntity>();
    public ICollection<FareEntity> Tarifas { get; set; } = new List<FareEntity>();
}