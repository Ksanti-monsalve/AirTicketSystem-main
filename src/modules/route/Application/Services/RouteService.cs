// src/modules/route/Application/Services/RouteService.cs
using AirTicketSystem.modules.route.Application.Interfaces;
using AirTicketSystem.modules.route.Application.UseCases;
using AirTicketSystem.modules.route.Domain.aggregate;

namespace AirTicketSystem.modules.route.Application.Services;

public class RouteService : IRouteService
{
    private readonly CreateRouteUseCase _create;
    private readonly GetRouteByIdUseCase _getById;
    private readonly GetAllRoutesUseCase _getAll;
    private readonly GetRoutesByAirlineUseCase _getByAirline;
    private readonly GetRoutesByOriginUseCase _getByOrigin;
    private readonly GetRoutesByDestinationUseCase _getByDestination;
    private readonly SearchRoutesUseCase _search;
    private readonly GetActiveRoutesUseCase _getActivas;
    private readonly UpdateRouteUseCase _update;
    private readonly ActivateRouteUseCase _activate;
    private readonly DeactivateRouteUseCase _deactivate;
    private readonly DeleteRouteUseCase _delete;

    public RouteService(
        CreateRouteUseCase create,
        GetRouteByIdUseCase getById,
        GetAllRoutesUseCase getAll,
        GetRoutesByAirlineUseCase getByAirline,
        GetRoutesByOriginUseCase getByOrigin,
        GetRoutesByDestinationUseCase getByDestination,
        SearchRoutesUseCase search,
        GetActiveRoutesUseCase getActivas,
        UpdateRouteUseCase update,
        ActivateRouteUseCase activate,
        DeactivateRouteUseCase deactivate,
        DeleteRouteUseCase delete)
    {
        _create           = create;
        _getById          = getById;
        _getAll           = getAll;
        _getByAirline     = getByAirline;
        _getByOrigin      = getByOrigin;
        _getByDestination = getByDestination;
        _search           = search;
        _getActivas       = getActivas;
        _update           = update;
        _activate         = activate;
        _deactivate       = deactivate;
        _delete           = delete;
    }

    public Task<Route> CreateAsync(
        int aerolineaId, int origenId, int destinoId,
        int? distanciaKm, int? duracionMin)
        => _create.ExecuteAsync(
            aerolineaId, origenId, destinoId, distanciaKm, duracionMin);

    public Task<Route> GetByIdAsync(int id)
        => _getById.ExecuteAsync(id);

    public Task<IReadOnlyCollection<Route>> GetAllAsync()
        => _getAll.ExecuteAsync();

    public Task<IReadOnlyCollection<Route>> GetByAirlineAsync(int aerolineaId)
        => _getByAirline.ExecuteAsync(aerolineaId);

    public Task<IReadOnlyCollection<Route>> GetByOriginAsync(int origenId)
        => _getByOrigin.ExecuteAsync(origenId);

    public Task<IReadOnlyCollection<Route>> GetByDestinationAsync(int destinoId)
        => _getByDestination.ExecuteAsync(destinoId);

    public Task<IReadOnlyCollection<Route>> SearchAsync(int origenId, int destinoId)
        => _search.ExecuteAsync(origenId, destinoId);

    public Task<IReadOnlyCollection<Route>> GetActivasAsync()
        => _getActivas.ExecuteAsync();

    public Task<Route> UpdateAsync(int id, int? distanciaKm, int? duracionMin)
        => _update.ExecuteAsync(id, distanciaKm, duracionMin);

    public Task ActivateAsync(int id) => _activate.ExecuteAsync(id);
    public Task DeactivateAsync(int id) => _deactivate.ExecuteAsync(id);
    public Task DeleteAsync(int id) => _delete.ExecuteAsync(id);
}