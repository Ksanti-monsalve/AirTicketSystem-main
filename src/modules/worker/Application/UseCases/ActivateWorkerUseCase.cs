// src/modules/worker/Application/UseCases/ActivateWorkerUseCase.cs
using AirTicketSystem.modules.worker.Domain.aggregate;
using AirTicketSystem.modules.worker.Domain.Repositories;

namespace AirTicketSystem.modules.worker.Application.UseCases;

public sealed class ActivateWorkerUseCase
{
    private readonly IWorkerRepository _repository;

    public ActivateWorkerUseCase(IWorkerRepository repository) => _repository = repository;

    public async Task<Worker> ExecuteAsync(int id, CancellationToken cancellationToken = default)
    {
        if (id <= 0)
            throw new ArgumentException("El ID del trabajador no es válido.");

        var worker = await _repository.FindByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un trabajador con ID {id}.");

        worker.Activar();
        await _repository.UpdateAsync(worker);
        return worker;
    }
}
