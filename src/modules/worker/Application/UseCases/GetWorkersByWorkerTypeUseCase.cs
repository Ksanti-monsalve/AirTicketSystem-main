// src/modules/worker/Application/UseCases/GetWorkersByWorkerTypeUseCase.cs
using AirTicketSystem.modules.worker.Domain.aggregate;
using AirTicketSystem.modules.worker.Domain.Repositories;

namespace AirTicketSystem.modules.worker.Application.UseCases;

public sealed class GetWorkersByWorkerTypeUseCase
{
    private readonly IWorkerRepository _repository;

    public GetWorkersByWorkerTypeUseCase(IWorkerRepository repository)
        => _repository = repository;

    public async Task<IReadOnlyCollection<Worker>> ExecuteAsync(
        int tipoTrabajadorId, CancellationToken cancellationToken = default)
    {
        if (tipoTrabajadorId <= 0)
            throw new ArgumentException("El ID del tipo de trabajador no es válido.");

        return await _repository.FindByTipoTrabajadorAsync(tipoTrabajadorId);
    }
}
