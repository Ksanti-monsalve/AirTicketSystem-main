// src/modules/city/Domain/Repositories/ICityRepository.cs
using AirTicketSystem.modules.city.Domain.aggregate;

namespace AirTicketSystem.modules.city.Domain.Repositories;

public interface ICityRepository
{
    Task<City?> FindByIdAsync(int id);
    Task<IReadOnlyCollection<City>> FindAllAsync();
    Task<IReadOnlyCollection<City>> FindByDepartamentoAsync(int departamentoId);
    Task<bool> ExistsByNombreAndDepartamentoAsync(string nombre, int departamentoId);
    Task SaveAsync(City city);
    Task UpdateAsync(City city);
    Task DeleteAsync(int id);
}
