// src/modules/workertype/Application/UseCases/DeleteWorkerTypeUseCase.cs
using AirTicketSystem.modules.workertype.Domain.Repositories;

namespace AirTicketSystem.modules.workertype.Application.UseCases;

public class DeleteWorkerTypeUseCase
{
    private readonly IWorkerTypeRepository _repository;

    public DeleteWorkerTypeUseCase(IWorkerTypeRepository repository)
    {
        _repository = repository;
    }

    public async Task ExecuteAsync(int id)
    {
        _ = await _repository.FindByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un tipo de trabajador con ID {id}.");

        await _repository.DeleteAsync(id);
    }
}