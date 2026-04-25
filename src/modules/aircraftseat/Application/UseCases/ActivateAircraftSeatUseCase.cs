// src/modules/aircraftseat/Application/UseCases/ActivateAircraftSeatUseCase.cs
using AirTicketSystem.modules.aircraftseat.Domain.Repositories;

namespace AirTicketSystem.modules.aircraftseat.Application.UseCases;

public class ActivateAircraftSeatUseCase
{
    private readonly IAircraftSeatRepository _repository;

    public ActivateAircraftSeatUseCase(IAircraftSeatRepository repository)
    {
        _repository = repository;
    }

    public async Task ExecuteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var asiento = await _repository.FindByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un asiento con ID {id}.");

        asiento.Activar();
        await _repository.UpdateAsync(asiento);
    }
}