// src/modules/workertype/Application/UseCases/CreateWorkerTypeUseCase.cs
using AirTicketSystem.modules.workertype.Domain.Repositories;
using AirTicketSystem.modules.workertype.Domain.aggregate;
using AirTicketSystem.modules.workertype.Domain.ValueObjects;

namespace AirTicketSystem.modules.workertype.Application.UseCases;

public class CreateWorkerTypeUseCase
{
    private readonly IWorkerTypeRepository _repository;

    public CreateWorkerTypeUseCase(IWorkerTypeRepository repository)
    {
        _repository = repository;
    }

    public async Task<WorkerType> ExecuteAsync(
        string nombre,
        CancellationToken cancellationToken = default)
    {
        var nombreVO = NombreWorkerType.Crear(nombre);

        if (await _repository.ExistsByNombreAsync(nombreVO.Valor))
            throw new InvalidOperationException(
                $"Ya existe un tipo de trabajador con el nombre '{nombreVO.Valor}'.");

        var workerType = WorkerType.Crear(nombreVO.Valor);
        await _repository.SaveAsync(workerType);
        return workerType;
    }
}