// src/modules/worker/Application/UseCases/GetWorkersByAirportUseCase.cs
using AirTicketSystem.modules.worker.Domain.aggregate;
using AirTicketSystem.modules.worker.Domain.Repositories;

namespace AirTicketSystem.modules.worker.Application.UseCases;

public sealed class GetWorkersByAirportUseCase
{
    private readonly IWorkerRepository _repository;

    public GetWorkersByAirportUseCase(IWorkerRepository repository) => _repository = repository;

    public async Task<IReadOnlyCollection<Worker>> ExecuteAsync(
        int aeropuertoId, CancellationToken cancellationToken = default)
    {
        if (aeropuertoId <= 0)
            throw new ArgumentException("El ID del aeropuerto no es válido.");

        return await _repository.FindByAeropuertoBaseAsync(aeropuertoId);
    }
}
