// src/modules/airline/Application/UseCases/GetActiveAirlinesUseCase.cs
using AirTicketSystem.modules.airline.Domain.Repositories;
using AirTicketSystem.modules.airline.Domain.aggregate;

namespace AirTicketSystem.modules.airline.Application.UseCases;

public class GetActiveAirlinesUseCase
{
    private readonly IAirlineRepository _repository;

    public GetActiveAirlinesUseCase(IAirlineRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyCollection<Airline>> ExecuteAsync(
        CancellationToken cancellationToken = default)
        => await _repository.FindActivasAsync();
}