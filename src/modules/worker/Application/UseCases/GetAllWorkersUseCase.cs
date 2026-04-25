// src/modules/worker/Application/UseCases/GetAllWorkersUseCase.cs
using AirTicketSystem.modules.worker.Domain.aggregate;
using AirTicketSystem.modules.worker.Domain.Repositories;

namespace AirTicketSystem.modules.worker.Application.UseCases;

public sealed class GetAllWorkersUseCase
{
    private readonly IWorkerRepository _repository;

    public GetAllWorkersUseCase(IWorkerRepository repository) => _repository = repository;

    public Task<IReadOnlyCollection<Worker>> ExecuteAsync(
        CancellationToken cancellationToken = default)
        => _repository.FindAllAsync();
}
