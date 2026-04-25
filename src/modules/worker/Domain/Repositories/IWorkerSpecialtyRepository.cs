// src/modules/worker/Domain/Repositories/IWorkerSpecialtyRepository.cs
using AirTicketSystem.modules.worker.Domain.aggregate;

namespace AirTicketSystem.modules.worker.Domain.Repositories;

public interface IWorkerSpecialtyRepository
{
    Task<WorkerSpecialty?> FindByIdAsync(int id);
    Task<IReadOnlyCollection<WorkerSpecialty>> FindByTrabajadorAsync(int trabajadorId);
    Task<IReadOnlyCollection<WorkerSpecialty>> FindByEspecialidadAsync(int especialidadId);
    Task<bool> ExistsByTrabajadorAndEspecialidadAsync(int trabajadorId, int especialidadId);
    Task SaveAsync(WorkerSpecialty specialty);
    Task DeleteAsync(int id);
}
