// src/modules/flight/Application/UseCases/GetFlightsWithOpenCheckinUseCase.cs
using AirTicketSystem.modules.flight.Domain.Repositories;
using AirTicketSystem.modules.flight.Domain.aggregate;

namespace AirTicketSystem.modules.flight.Application.UseCases;

public sealed class GetFlightsWithOpenCheckinUseCase
{
    private readonly IFlightRepository _repository;

    public GetFlightsWithOpenCheckinUseCase(IFlightRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyCollection<Flight>> ExecuteAsync(
        CancellationToken cancellationToken = default)
        => await _repository.FindConCheckinAbiertoAsync();
}