// src/modules/flight/Application/UseCases/GetAllFlightsUseCase.cs
using AirTicketSystem.modules.flight.Domain.Repositories;
using AirTicketSystem.modules.flight.Domain.aggregate;

namespace AirTicketSystem.modules.flight.Application.UseCases;

public sealed class GetAllFlightsUseCase
{
    private readonly IFlightRepository _repository;

    public GetAllFlightsUseCase(IFlightRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyCollection<Flight>> ExecuteAsync(
        CancellationToken cancellationToken = default)
        => await _repository.FindAllAsync();
}