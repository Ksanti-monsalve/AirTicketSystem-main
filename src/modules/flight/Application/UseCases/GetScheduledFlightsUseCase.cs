// src/modules/flight/Application/UseCases/GetScheduledFlightsUseCase.cs
using AirTicketSystem.modules.flight.Domain.Repositories;
using AirTicketSystem.modules.flight.Domain.aggregate;

namespace AirTicketSystem.modules.flight.Application.UseCases;

public sealed class GetScheduledFlightsUseCase
{
    private readonly IFlightRepository _repository;

    public GetScheduledFlightsUseCase(IFlightRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyCollection<Flight>> ExecuteAsync(
        CancellationToken cancellationToken = default)
        => await _repository.FindProgramadosAsync();
}