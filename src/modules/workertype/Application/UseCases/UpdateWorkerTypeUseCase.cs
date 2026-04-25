// src/modules/workertype/Application/UseCases/UpdateWorkerTypeUseCase.cs
using AirTicketSystem.modules.workertype.Domain.Repositories;
using AirTicketSystem.modules.workertype.Domain.aggregate;

namespace AirTicketSystem.modules.workertype.Application.UseCases;

public class UpdateWorkerTypeUseCase
{
    private readonly IWorkerTypeRepository _repository;

    public UpdateWorkerTypeUseCase(IWorkerTypeRepository repository)
    {
        _repository = repository;
    }

    public async Task<WorkerType> ExecuteAsync(
        int id,
        string nombre,
        CancellationToken cancellationToken = default)
    {
        var tipo = await _repository.FindByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un tipo de trabajador con ID {id}.");

        if (!string.Equals(nombre.Trim(), tipo.Nombre.Valor, StringComparison.OrdinalIgnoreCase) &&
            await _repository.ExistsByNombreAsync(nombre))
            throw new InvalidOperationException(
                $"Ya existe otro tipo de trabajador con el nombre '{nombre.Trim()}'.");

        tipo.ActualizarNombre(nombre);
        await _repository.UpdateAsync(tipo);
        return tipo;
    }
}