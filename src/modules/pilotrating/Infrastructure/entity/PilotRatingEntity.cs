// src/modules/pilotrating/Infrastructure/entity/PilotRatingEntity.cs
using AirTicketSystem.modules.aircraftmodel.Infrastructure.entity;
using AirTicketSystem.modules.pilotlicense.Infrastructure.entity;

namespace AirTicketSystem.modules.pilotrating.Infrastructure.entity;

public class PilotRatingEntity
{
    public int Id { get; set; }
    public int LicenciaId { get; set; }
    public int ModeloAvionId { get; set; }
    public DateOnly FechaHabilitacion { get; set; }
    public DateOnly FechaVencimiento { get; set; }

    public PilotLicenseEntity Licencia { get; set; } = null!;
    public AircraftModelEntity ModeloAvion { get; set; } = null!;
}