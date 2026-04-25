// src/modules/worker/Application/UseCases/GetQualifiedPilotsUseCase.cs
using AirTicketSystem.modules.worker.Domain.aggregate;
using AirTicketSystem.modules.worker.Domain.Repositories;

namespace AirTicketSystem.modules.worker.Application.UseCases;

public sealed class GetQualifiedPilotsUseCase
{
    private readonly IWorkerRepository _repository;

    public GetQualifiedPilotsUseCase(IWorkerRepository repository) => _repository = repository;

    public async Task<IReadOnlyCollection<Worker>> ExecuteAsync(
        int modeloAvionId, CancellationToken cancellationToken = default)
    {
        if (modeloAvionId <= 0)
            throw new ArgumentException("El ID del modelo de avión no es válido.");

        return await _repository.FindPilotosHabilitadosParaModeloAsync(modeloAvionId);
    }
}
