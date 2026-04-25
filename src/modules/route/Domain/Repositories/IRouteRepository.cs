// src/modules/route/Domain/Repositories/IRouteRepository.cs
using AirTicketSystem.modules.route.Domain.aggregate;

namespace AirTicketSystem.modules.route.Domain.Repositories;

public interface IRouteRepository
{
    Task<Route?> FindByIdAsync(int id);
    Task<IReadOnlyCollection<Route>> FindAllAsync();
    Task<IReadOnlyCollection<Route>> FindByAerolineaAsync(int aerolineaId);
    Task<IReadOnlyCollection<Route>> FindByOrigenAsync(int origenId);
    Task<IReadOnlyCollection<Route>> FindByDestinoAsync(int destinoId);
    Task<IReadOnlyCollection<Route>> FindByOrigenAndDestinoAsync(
        int origenId, int destinoId);
    Task<Route?> FindByAerolineaOrigenDestinoAsync(
        int aerolineaId, int origenId, int destinoId);
    Task<IReadOnlyCollection<Route>> FindActivasAsync();
    Task<bool> ExistsByAerolineaOrigenDestinoAsync(
        int aerolineaId, int origenId, int destinoId);
    Task SaveAsync(Route route);
    Task UpdateAsync(Route route);
    Task DeleteAsync(int id);
}