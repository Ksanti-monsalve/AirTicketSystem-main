// src/modules/flight/Application/UseCases/GetFlightByIdUseCase.cs
using AirTicketSystem.modules.flight.Domain.Repositories;
using AirTicketSystem.modules.flight.Domain.aggregate;

namespace AirTicketSystem.modules.flight.Application.UseCases;

public sealed class GetFlightByIdUseCase
{
    private readonly IFlightRepository _repository;

    public GetFlightByIdUseCase(IFlightRepository repository)
    {
        _repository = repository;
    }

    public async Task<Flight> ExecuteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        if (id <= 0)
            throw new ArgumentException("El ID del vuelo no es válido.");

        return await _repository.FindByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un vuelo con ID {id}.");
    }
}