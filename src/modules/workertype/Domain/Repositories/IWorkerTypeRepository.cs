// src/modules/workertype/Domain/Repositories/IWorkerTypeRepository.cs
using AirTicketSystem.modules.workertype.Domain.aggregate;

namespace AirTicketSystem.modules.workertype.Domain.Repositories;

public interface IWorkerTypeRepository
{
    Task<WorkerType?> FindByIdAsync(int id);
    Task<IReadOnlyCollection<WorkerType>> FindAllAsync();
    Task<WorkerType?> FindByNombreAsync(string nombre);
    Task<bool> ExistsByNombreAsync(string nombre);
    Task SaveAsync(WorkerType workerType);
    Task UpdateAsync(WorkerType workerType);
    Task DeleteAsync(int id);
}