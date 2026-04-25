// src/modules/pilotlicense/Infrastructure/entity/PilotLicenseEntity.cs
using AirTicketSystem.modules.pilotrating.Infrastructure.entity;
using AirTicketSystem.modules.worker.Infrastructure.entity;

namespace AirTicketSystem.modules.pilotlicense.Infrastructure.entity;

public class PilotLicenseEntity
{
    public int Id { get; set; }
    public int TrabajadorId { get; set; }
    public string NumeroLicencia { get; set; } = null!;
    public string TipoLicencia { get; set; } = null!;
    public DateOnly FechaExpedicion { get; set; }
    public DateOnly FechaVencimiento { get; set; }
    public string AutoridadEmisora { get; set; } = null!;
    public bool Activa { get; set; } = true;

    public WorkerEntity Trabajador { get; set; } = null!;
    public ICollection<PilotRatingEntity> Habilitaciones { get; set; } = new List<PilotRatingEntity>();
}