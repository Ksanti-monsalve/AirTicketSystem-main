// src/modules/workertype/Infrastructure/entity/WorkerTypeEntity.cs
using AirTicketSystem.modules.specialty.Infrastructure.entity;
using AirTicketSystem.modules.worker.Infrastructure.entity;

namespace AirTicketSystem.modules.workertype.Infrastructure.entity;

public class WorkerTypeEntity
{
    public int Id { get; set; }
    public string Nombre { get; set; } = null!;

    public ICollection<WorkerEntity> Trabajadores { get; set; } = new List<WorkerEntity>();
    public ICollection<SpecialtyEntity> Especialidades { get; set; } = new List<SpecialtyEntity>();
}