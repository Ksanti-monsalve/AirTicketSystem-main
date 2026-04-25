// src/modules/workertype/Application/Interfaces/IWorkerTypeService.cs
using AirTicketSystem.modules.workertype.Domain.aggregate;

namespace AirTicketSystem.modules.workertype.Application.Interfaces;

public interface IWorkerTypeService
{
    Task<WorkerType> CreateAsync(string nombre);
    Task<WorkerType> GetByIdAsync(int id);
    Task<IReadOnlyCollection<WorkerType>> GetAllAsync();
    Task<WorkerType> UpdateAsync(int id, string nombre);
    Task DeleteAsync(int id);
}