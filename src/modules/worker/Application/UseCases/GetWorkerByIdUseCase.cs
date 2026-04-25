// src/modules/worker/Application/UseCases/GetWorkerByIdUseCase.cs
using AirTicketSystem.modules.worker.Domain.aggregate;
using AirTicketSystem.modules.worker.Domain.Repositories;

namespace AirTicketSystem.modules.worker.Application.UseCases;

public sealed class GetWorkerByIdUseCase
{
    private readonly IWorkerRepository _repository;

    public GetWorkerByIdUseCase(IWorkerRepository repository) => _repository = repository;

    public async Task<Worker> ExecuteAsync(int id, CancellationToken cancellationToken = default)
    {
        if (id <= 0)
            throw new ArgumentException("El ID del trabajador no es válido.");

        return await _repository.FindByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un trabajador con ID {id}.");
    }
}
