// src/modules/route/Application/UseCases/GetAllRoutesUseCase.cs
using AirTicketSystem.modules.route.Domain.Repositories;
using AirTicketSystem.modules.route.Domain.aggregate;

namespace AirTicketSystem.modules.route.Application.UseCases;

public class GetAllRoutesUseCase
{
    private readonly IRouteRepository _repository;

    public GetAllRoutesUseCase(IRouteRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyCollection<Route>> ExecuteAsync(
        CancellationToken cancellationToken = default)
        => await _repository.FindAllAsync();
}