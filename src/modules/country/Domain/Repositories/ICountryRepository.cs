// src/modules/country/Domain/Repositories/ICountryRepository.cs
using AirTicketSystem.modules.country.Domain.aggregate;

namespace AirTicketSystem.modules.country.Domain.Repositories;

public interface ICountryRepository
{
    Task<Country?> FindByIdAsync(int id);
    Task<IReadOnlyCollection<Country>> FindAllAsync();
    Task<IReadOnlyCollection<Country>> FindByContinenteAsync(int continenteId);
    Task<bool> ExistsByCodigoIso2Async(string codigoIso2);
    Task<bool> ExistsByCodigoIso3Async(string codigoIso3);
    Task SaveAsync(Country country);
    Task UpdateAsync(Country country);
    Task DeleteAsync(int id);
}
