// src/modules/region/Application/Interfaces/IRegionService.cs
using AirTicketSystem.modules.region.Domain.aggregate;

namespace AirTicketSystem.modules.region.Application.Interfaces;

public interface IRegionService
{
    Task<Region> CreateAsync(int paisId, string nombre, string? codigo);
    Task<Region> GetByIdAsync(int id);
    Task<IReadOnlyCollection<Region>> GetAllAsync();
    Task<IReadOnlyCollection<Region>> GetByCountryAsync(int paisId);
    Task<Region> UpdateAsync(int id, string nombre, string? codigo);
    Task DeleteAsync(int id);
}
