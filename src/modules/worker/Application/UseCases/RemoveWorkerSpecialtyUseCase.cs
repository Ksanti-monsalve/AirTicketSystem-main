// src/modules/worker/Application/UseCases/RemoveWorkerSpecialtyUseCase.cs
using AirTicketSystem.modules.worker.Domain.Repositories;

namespace AirTicketSystem.modules.worker.Application.UseCases;

public sealed class RemoveWorkerSpecialtyUseCase
{
    private readonly IWorkerSpecialtyRepository _repository;

    public RemoveWorkerSpecialtyUseCase(IWorkerSpecialtyRepository repository)
        => _repository = repository;

    public async Task ExecuteAsync(
        int workerSpecialtyId, CancellationToken cancellationToken = default)
    {
        if (workerSpecialtyId <= 0)
            throw new ArgumentException(
                "El ID de la asignación de especialidad no es válido.");

        _ = await _repository.FindByIdAsync(workerSpecialtyId)
            ?? throw new KeyNotFoundException(
                $"No se encontró la asignación de especialidad con ID {workerSpecialtyId}.");

        await _repository.DeleteAsync(workerSpecialtyId);
    }
}
