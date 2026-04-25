// src/modules/flightcrew/Application/UseCases/DeleteFlightCrewUseCase.cs
using AirTicketSystem.modules.flightcrew.Domain.Repositories;

namespace AirTicketSystem.modules.flightcrew.Application.UseCases;

public sealed class DeleteFlightCrewUseCase
{
    private readonly IFlightCrewRepository _repository;

    public DeleteFlightCrewUseCase(IFlightCrewRepository repository)
    {
        _repository = repository;
    }

    public async Task ExecuteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        _ = await _repository.FindByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró la asignación de tripulación con ID {id}.");

        await _repository.DeleteAsync(id);
    }
}