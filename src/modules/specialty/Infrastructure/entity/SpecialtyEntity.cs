// src/modules/specialty/Infrastructure/entity/SpecialtyEntity.cs
using AirTicketSystem.modules.worker.Infrastructure.entity;
using AirTicketSystem.modules.workertype.Infrastructure.entity;

namespace AirTicketSystem.modules.specialty.Infrastructure.entity;

public class SpecialtyEntity
{
    public int Id { get; set; }
    public string Nombre { get; set; } = null!;
    public int? TipoTrabajadorId { get; set; }

    public WorkerTypeEntity? TipoTrabajador { get; set; }
    public ICollection<WorkerSpecialtyEntity> TrabajadorEspecialidades { get; set; } = new List<WorkerSpecialtyEntity>();
}