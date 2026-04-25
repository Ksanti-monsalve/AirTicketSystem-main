// src/modules/airline/Domain/Repositories/IAirlineRepository.cs
using AirTicketSystem.modules.airline.Domain.aggregate;

namespace AirTicketSystem.modules.airline.Domain.Repositories;

public interface IAirlineRepository
{
    Task<Airline?> FindByIdAsync(int id);
    Task<IReadOnlyCollection<Airline>> FindAllAsync();
    Task<Airline?> FindByCodigoIataAsync(string codigoIata);
    Task<Airline?> FindByCodigoIcaoAsync(string codigoIcao);
    Task<IReadOnlyCollection<Airline>> FindActivasAsync();
    Task<bool> ExistsByCodigoIataAsync(string codigoIata);
    Task<bool> ExistsByCodigoIcaoAsync(string codigoIcao);
    Task SaveAsync(Airline airline);
    Task UpdateAsync(Airline airline);
    Task DeleteAsync(int id);
}