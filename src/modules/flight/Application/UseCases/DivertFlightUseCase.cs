// src/modules/flight/Application/UseCases/DivertFlightUseCase.cs
using AirTicketSystem.modules.flight.Domain.Repositories;

namespace AirTicketSystem.modules.flight.Application.UseCases;

public sealed class DivertFlightUseCase
{
    private readonly IFlightRepository _repository;

    public DivertFlightUseCase(IFlightRepository repository)
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

        flight.Desviar(motivo);

        await _repository.UpdateAsync(flight);
    }
}