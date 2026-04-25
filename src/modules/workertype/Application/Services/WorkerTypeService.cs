// src/modules/workertype/Application/Services/WorkerTypeService.cs
using AirTicketSystem.modules.workertype.Application.Interfaces;
using AirTicketSystem.modules.workertype.Application.UseCases;
using AirTicketSystem.modules.workertype.Domain.aggregate;

namespace AirTicketSystem.modules.workertype.Application.Services;

public class WorkerTypeService : IWorkerTypeService
{
    private readonly CreateWorkerTypeUseCase _create;
    private readonly GetWorkerTypeByIdUseCase _getById;
    private readonly GetAllWorkerTypesUseCase _getAll;
    private readonly UpdateWorkerTypeUseCase _update;
    private readonly DeleteWorkerTypeUseCase _delete;

    public WorkerTypeService(
        CreateWorkerTypeUseCase create,
        GetWorkerTypeByIdUseCase getById,
        GetAllWorkerTypesUseCase getAll,
        UpdateWorkerTypeUseCase update,
        DeleteWorkerTypeUseCase delete)
    {
        _create  = create;
        _getById = getById;
        _getAll  = getAll;
        _update  = update;
        _delete  = delete;
    }

    public Task<WorkerType> CreateAsync(string nombre)
        => _create.ExecuteAsync(nombre);

    public Task<WorkerType> GetByIdAsync(int id)
        => _getById.ExecuteAsync(id);

    public Task<IReadOnlyCollection<WorkerType>> GetAllAsync()
        => _getAll.ExecuteAsync();

    public Task<WorkerType> UpdateAsync(int id, string nombre)
        => _update.ExecuteAsync(id, nombre);

    public Task DeleteAsync(int id)
        => _delete.ExecuteAsync(id);
}