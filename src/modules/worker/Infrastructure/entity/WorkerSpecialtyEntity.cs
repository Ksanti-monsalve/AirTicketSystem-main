// src/modules/worker/Infrastructure/entity/WorkerSpecialtyEntity.cs
using AirTicketSystem.modules.specialty.Infrastructure.entity;

namespace AirTicketSystem.modules.worker.Infrastructure.entity;

public class WorkerSpecialtyEntity
{
    public int Id { get; set; }
    public int TrabajadorId { get; set; }
    public int EspecialidadId { get; set; }

    public WorkerEntity Trabajador { get; set; } = null!;
    public SpecialtyEntity Especialidad { get; set; } = null!;
}