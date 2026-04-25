// src/modules/route/Application/UseCases/GetRoutesByAirlineUseCase.cs
using AirTicketSystem.modules.route.Domain.Repositories;
using AirTicketSystem.modules.route.Domain.aggregate;

namespace AirTicketSystem.modules.route.Application.UseCases;

public class GetRoutesByAirlineUseCase
{
    private readonly IRouteRepository _repository;

    public GetRoutesByAirlineUseCase(IRouteRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyCollection<Route>> ExecuteAsync(
        int aerolineaId,
        CancellationToken cancellationToken = default)
    {
        if (aerolineaId <= 0)
            throw new ArgumentException("El ID de la aerolínea no es válido.");

        return await _repository.FindByAerolineaAsync(aerolineaId);
    }
}