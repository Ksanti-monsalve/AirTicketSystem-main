// src/modules/continent/Application/Interfaces/IContinentService.cs
using AirTicketSystem.modules.continent.Domain.aggregate;

namespace AirTicketSystem.modules.continent.Application.Interfaces;

public interface IContinentService
{
    Task<Continent> CreateAsync(string nombre, string codigo);
    Task<Continent> GetByIdAsync(int id);
    Task<IReadOnlyCollection<Continent>> GetAllAsync();
    Task<Continent> UpdateAsync(int id, string nombre, string codigo);
    Task DeleteAsync(int id);
}
