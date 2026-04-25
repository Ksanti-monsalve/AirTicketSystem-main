// src/modules/worker/Application/UseCases/GetWorkersByAirlineUseCase.cs
using AirTicketSystem.modules.worker.Domain.aggregate;
using AirTicketSystem.modules.worker.Domain.Repositories;

namespace AirTicketSystem.modules.worker.Application.UseCases;

public sealed class GetWorkersByAirlineUseCase
{
    private readonly IWorkerRepository _repository;

    public GetWorkersByAirlineUseCase(IWorkerRepository repository) => _repository = repository;

    public async Task<IReadOnlyCollection<Worker>> ExecuteAsync(
        int aerolineaId, CancellationToken cancellationToken = default)
    {
        if (aerolineaId <= 0)
            throw new ArgumentException("El ID de la aerolínea no es válido.");

        return await _repository.FindByAerolineaAsync(aerolineaId);
    }
}
