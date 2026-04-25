// src/modules/flight/Application/UseCases/CancelFlightUseCase.cs
using AirTicketSystem.modules.flight.Domain.Repositories;

namespace AirTicketSystem.modules.flight.Application.UseCases;

public sealed class CancelFlightUseCase
{
    private readonly IFlightRepository _repository;

    public CancelFlightUseCase(IFlightRepository repository)
    {
        _repository = repository;
    }

    public async Task ExecuteAsync(
        int id,
        string motivo,
        CancellationToken cancellationToken = default)
    {
        var flight = await _repository.FindByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un vuelo con ID {id}.");

        // El aggregate valida las transiciones de estado
        flight.Cancelar(motivo);

        await _repository.UpdateAsync(flight);
    }
}