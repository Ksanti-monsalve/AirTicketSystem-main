// src/modules/aircraft/Application/UseCases/ReactivateAircraftUseCase.cs
using AirTicketSystem.modules.aircraft.Domain.Repositories;

namespace AirTicketSystem.modules.aircraft.Application.UseCases;

public class ReactivateAircraftUseCase
{
    private readonly IAircraftRepository _repository;

    public ReactivateAircraftUseCase(IAircraftRepository repository)
    {
        _repository = repository;
    }

    public async Task ExecuteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var avion = await _repository.FindByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un avión con ID {id}.");
        avion.Reactivar();

        await _repository.UpdateAsync(avion);
    }
}