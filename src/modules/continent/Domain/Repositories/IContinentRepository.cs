// src/modules/continent/Domain/Repositories/IContinentRepository.cs
using AirTicketSystem.modules.continent.Domain.aggregate;

namespace AirTicketSystem.modules.continent.Domain.Repositories;

public interface IContinentRepository
{
    Task<Continent?> FindByIdAsync(int id);
    Task<IReadOnlyCollection<Continent>> FindAllAsync();
    Task<Continent?> FindByCodigoAsync(string codigo);
    Task<bool> ExistsByCodigoAsync(string codigo);
    Task<bool> ExistsByNombreAsync(string nombre);
    Task SaveAsync(Continent continent);
    Task UpdateAsync(Continent continent);
    Task DeleteAsync(int id);
}
