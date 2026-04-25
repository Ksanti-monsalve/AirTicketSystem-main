// src/modules/workertype/Application/UseCases/GetWorkerTypeByIdUseCase.cs
using AirTicketSystem.modules.workertype.Domain.Repositories;
using AirTicketSystem.modules.workertype.Domain.aggregate;

namespace AirTicketSystem.modules.workertype.Application.UseCases;

public class GetWorkerTypeByIdUseCase
{
    private readonly IWorkerTypeRepository _repository;

    public GetWorkerTypeByIdUseCase(IWorkerTypeRepository repository)
    {
        _repository = repository;
    }

    public async Task<WorkerType> ExecuteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        if (id <= 0)
            throw new ArgumentException(
                "El ID del tipo de trabajador no es válido.");

        return await _repository.FindByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un tipo de trabajador con ID {id}.");
    }
}