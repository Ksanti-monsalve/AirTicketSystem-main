// src/modules/worker/Application/UseCases/GetActiveWorkersUseCase.cs
using AirTicketSystem.modules.worker.Domain.aggregate;
using AirTicketSystem.modules.worker.Domain.Repositories;

namespace AirTicketSystem.modules.worker.Application.UseCases;

public sealed class GetActiveWorkersUseCase
{
    private readonly IWorkerRepository _repository;

    public GetActiveWorkersUseCase(IWorkerRepository repository) => _repository = repository;

    public Task<IReadOnlyCollection<Worker>> ExecuteAsync(
        CancellationToken cancellationToken = default)
        => _repository.FindActivosAsync();
}
