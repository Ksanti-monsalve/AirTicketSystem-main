// src/modules/worker/Application/UseCases/UpdateWorkerSalaryUseCase.cs
using AirTicketSystem.modules.worker.Domain.aggregate;
using AirTicketSystem.modules.worker.Domain.Repositories;

namespace AirTicketSystem.modules.worker.Application.UseCases;

public sealed class UpdateWorkerSalaryUseCase
{
    private readonly IWorkerRepository _repository;

    public UpdateWorkerSalaryUseCase(IWorkerRepository repository) => _repository = repository;

    public async Task<Worker> ExecuteAsync(
        int id, decimal nuevoSalario, CancellationToken cancellationToken = default)
    {
        if (id <= 0)
            throw new ArgumentException("El ID del trabajador no es válido.");

        var worker = await _repository.FindByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un trabajador con ID {id}.");

        if (!worker.EstaActivo)
            throw new InvalidOperationException(
                "No se puede actualizar el salario de un trabajador inactivo.");

        worker.ActualizarSalario(nuevoSalario);
        await _repository.UpdateAsync(worker);
        return worker;
    }
}
