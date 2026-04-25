// src/modules/workertype/Application/UseCases/GetAllWorkerTypesUseCase.cs
using AirTicketSystem.modules.workertype.Domain.Repositories;
using AirTicketSystem.modules.workertype.Domain.aggregate;

namespace AirTicketSystem.modules.workertype.Application.UseCases;

public class GetAllWorkerTypesUseCase
{
    private readonly IWorkerTypeRepository _repository;

    public GetAllWorkerTypesUseCase(IWorkerTypeRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyCollection<WorkerType>> ExecuteAsync(
        CancellationToken cancellationToken = default)
        => (await _repository.FindAllAsync())
            .OrderBy(w => w.Nombre.Valor)
            .ToList()
            .AsReadOnly();
}