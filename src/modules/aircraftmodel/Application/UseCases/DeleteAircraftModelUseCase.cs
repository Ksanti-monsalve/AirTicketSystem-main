// src/modules/aircraftmodel/Application/UseCases/DeleteAircraftModelUseCase.cs
using AirTicketSystem.modules.aircraftmodel.Domain.Repositories;

namespace AirTicketSystem.modules.aircraftmodel.Application.UseCases;

public class DeleteAircraftModelUseCase
{
    private readonly IAircraftModelRepository _repository;

    public DeleteAircraftModelUseCase(IAircraftModelRepository repository)
    {
        _repository = repository;
    }

    public async Task ExecuteAsync(int id)
    {
        _ = await _repository.FindByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un modelo de avión con ID {id}.");

        await _repository.DeleteAsync(id);
    }
}