// src/modules/region/Domain/Repositories/IRegionRepository.cs
using AirTicketSystem.modules.region.Domain.aggregate;

namespace AirTicketSystem.modules.region.Domain.Repositories;

public interface IRegionRepository
{
    Task<Region?> FindByIdAsync(int id);
    Task<IReadOnlyCollection<Region>> FindAllAsync();
    Task<IReadOnlyCollection<Region>> FindByPaisAsync(int paisId);
    Task<bool> ExistsByNombreAndPaisAsync(string nombre, int paisId);
    Task SaveAsync(Region region);
    Task UpdateAsync(Region region);
    Task DeleteAsync(int id);
}
