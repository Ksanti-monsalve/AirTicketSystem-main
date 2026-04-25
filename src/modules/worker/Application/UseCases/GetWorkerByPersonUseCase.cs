// src/modules/worker/Application/UseCases/GetWorkerByPersonUseCase.cs
using AirTicketSystem.modules.worker.Domain.aggregate;
using AirTicketSystem.modules.worker.Domain.Repositories;

namespace AirTicketSystem.modules.worker.Application.UseCases;

public sealed class GetWorkerByPersonUseCase
{
    private readonly IWorkerRepository _repository;

    public GetWorkerByPersonUseCase(IWorkerRepository repository) => _repository = repository;

    public async Task<Worker> ExecuteAsync(
        int personaId, CancellationToken cancellationToken = default)
    {
        if (personaId <= 0)
            throw new ArgumentException("El ID de la persona no es válido.");

        return await _repository.FindByPersonaAsync(personaId)
            ?? throw new KeyNotFoundException(
                $"No se encontró un trabajador para la persona con ID {personaId}.");
    }
}
