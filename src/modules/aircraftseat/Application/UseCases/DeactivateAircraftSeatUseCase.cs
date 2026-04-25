// src/modules/aircraftseat/Application/UseCases/DeactivateAircraftSeatUseCase.cs
using AirTicketSystem.modules.aircraftseat.Domain.Repositories;

namespace AirTicketSystem.modules.aircraftseat.Application.UseCases;

public class DeactivateAircraftSeatUseCase
{
    private readonly IAircraftSeatRepository _repository;

    public DeactivateAircraftSeatUseCase(IAircraftSeatRepository repository)
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

        asiento.Desactivar();
        await _repository.UpdateAsync(asiento);
    }
}