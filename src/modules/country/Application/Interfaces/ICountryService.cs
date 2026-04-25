// src/modules/country/Application/Interfaces/ICountryService.cs
using AirTicketSystem.modules.country.Domain.aggregate;

namespace AirTicketSystem.modules.country.Application.Interfaces;

public interface ICountryService
{
    Task<Country> CreateAsync(
        int continenteId, string nombre,
        string codigoIso2, string codigoIso3);
    Task<Country> GetByIdAsync(int id);
    Task<IReadOnlyCollection<Country>> GetAllAsync();
    Task<IReadOnlyCollection<Country>> GetByContinentAsync(int continenteId);
    Task<Country> UpdateAsync(
        int id, string nombre,
        string codigoIso2, string codigoIso3);
    Task DeleteAsync(int id);
}
