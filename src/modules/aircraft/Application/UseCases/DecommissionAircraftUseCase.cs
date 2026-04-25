// src/modules/aircraft/Application/UseCases/DecommissionAircraftUseCase.cs
using AirTicketSystem.modules.aircraft.Domain.Repositories;

namespace AirTicketSystem.modules.aircraft.Application.UseCases;

public class DecommissionAircraftUseCase
{
    private readonly IAircraftRepository _repository;

    public DecommissionAircraftUseCase(IAircraftRepository repository)
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
        avion.DarDeBaja();

        await _repository.UpdateAsync(avion);
    }
}