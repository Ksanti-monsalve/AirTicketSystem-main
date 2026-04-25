// src/modules/route/Application/UseCases/GetActiveRoutesUseCase.cs
using AirTicketSystem.modules.route.Domain.Repositories;
using AirTicketSystem.modules.route.Domain.aggregate;

namespace AirTicketSystem.modules.route.Application.UseCases;

public class GetActiveRoutesUseCase
{
    private readonly IRouteRepository _repository;

    public GetActiveRoutesUseCase(IRouteRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyCollection<Route>> ExecuteAsync(
        CancellationToken cancellationToken = default)
        => await _repository.FindActivasAsync();
}