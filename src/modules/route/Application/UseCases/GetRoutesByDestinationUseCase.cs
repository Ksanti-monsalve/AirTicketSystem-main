// src/modules/route/Application/UseCases/GetRoutesByDestinationUseCase.cs
using AirTicketSystem.modules.route.Domain.Repositories;
using AirTicketSystem.modules.route.Domain.aggregate;

namespace AirTicketSystem.modules.route.Application.UseCases;

public class GetRoutesByDestinationUseCase
{
    private readonly IRouteRepository _repository;

    public GetRoutesByDestinationUseCase(IRouteRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyCollection<Route>> ExecuteAsync(
        int destinoId,
        CancellationToken cancellationToken = default)
    {
        if (destinoId <= 0)
            throw new ArgumentException(
                "El ID del aeropuerto de destino no es válido.");

        return await _repository.FindByDestinoAsync(destinoId);
    }
}