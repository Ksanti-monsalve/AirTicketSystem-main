// src/modules/route/Application/UseCases/GetRoutesByOriginUseCase.cs
using AirTicketSystem.modules.route.Domain.Repositories;
using AirTicketSystem.modules.route.Domain.aggregate;

namespace AirTicketSystem.modules.route.Application.UseCases;

public class GetRoutesByOriginUseCase
{
    private readonly IRouteRepository _repository;

    public GetRoutesByOriginUseCase(IRouteRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyCollection<Route>> ExecuteAsync(
        int origenId,
        CancellationToken cancellationToken = default)
    {
        if (origenId <= 0)
            throw new ArgumentException(
                "El ID del aeropuerto de origen no es válido.");

        return await _repository.FindByOrigenAsync(origenId);
    }
}