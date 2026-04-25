// src/modules/serviceclass/Application/UseCases/DeleteServiceClassUseCase.cs
using AirTicketSystem.modules.serviceclass.Domain.Repositories;

namespace AirTicketSystem.modules.serviceclass.Application.UseCases;

public class DeleteServiceClassUseCase
{
    private readonly IServiceClassRepository _repository;

    public DeleteServiceClassUseCase(IServiceClassRepository repository)
    {
        _repository = repository;
    }

    public async Task ExecuteAsync(int id)
    {
        _ = await _repository.FindByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró una clase de servicio con ID {id}.");

        await _repository.DeleteAsync(id);
    }
}