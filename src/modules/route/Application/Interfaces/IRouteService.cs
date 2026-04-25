// src/modules/route/Application/Interfaces/IRouteService.cs
using AirTicketSystem.modules.route.Domain.aggregate;

namespace AirTicketSystem.modules.route.Application.Interfaces;

public interface IRouteService
{
    Task<Route> CreateAsync(
        int aerolineaId, int origenId, int destinoId,
        int? distanciaKm, int? duracionMin);
    Task<Route> GetByIdAsync(int id);
    Task<IReadOnlyCollection<Route>> GetAllAsync();
    Task<IReadOnlyCollection<Route>> GetByAirlineAsync(int aerolineaId);
    Task<IReadOnlyCollection<Route>> GetByOriginAsync(int origenId);
    Task<IReadOnlyCollection<Route>> GetByDestinationAsync(int destinoId);
    Task<IReadOnlyCollection<Route>> SearchAsync(int origenId, int destinoId);
    Task<IReadOnlyCollection<Route>> GetActivasAsync();
    Task<Route> UpdateAsync(
        int id, int? distanciaKm, int? duracionMin);
    Task ActivateAsync(int id);
    Task DeactivateAsync(int id);
    Task DeleteAsync(int id);
}