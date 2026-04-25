// src/modules/airport/Domain/Repositories/IAirportRepository.cs
using AirTicketSystem.modules.airport.Domain.aggregate;

namespace AirTicketSystem.modules.airport.Domain.Repositories;

public interface IAirportRepository
{
    Task<Airport?> FindByIdAsync(int id);
    Task<IReadOnlyCollection<Airport>> FindAllAsync();
    Task<Airport?> FindByCodigoIataAsync(string codigoIata);
    Task<Airport?> FindByCodigoIcaoAsync(string codigoIcao);
    Task<IReadOnlyCollection<Airport>> FindByCiudadAsync(int ciudadId);
    Task<IReadOnlyCollection<Airport>> FindActivosAsync();
    Task<bool> ExistsByCodigoIataAsync(string codigoIata);
    Task<bool> ExistsByCodigoIcaoAsync(string codigoIcao);
    Task SaveAsync(Airport airport);
    Task UpdateAsync(Airport airport);
    Task DeleteAsync(int id);
}