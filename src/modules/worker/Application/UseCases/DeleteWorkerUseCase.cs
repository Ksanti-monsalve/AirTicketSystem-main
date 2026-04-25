// src/modules/worker/Application/UseCases/DeleteWorkerUseCase.cs
using AirTicketSystem.modules.worker.Domain.Repositories;

namespace AirTicketSystem.modules.worker.Application.UseCases;

public sealed class DeleteWorkerUseCase
{
    private readonly IWorkerRepository _repository;

    public DeleteWorkerUseCase(IWorkerRepository repository) => _repository = repository;

    public async Task ExecuteAsync(int id, CancellationToken cancellationToken = default)
    {
        if (id <= 0)
            throw new ArgumentException("El ID del trabajador no es válido.");

        var worker = await _repository.FindByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un trabajador con ID {id}.");

        if (worker.EstaActivo)
            throw new InvalidOperationException(
                "No se puede eliminar un trabajador activo. Desactívelo primero.");

        await _repository.DeleteAsync(id);
    }
}
