// src/modules/role/Domain/Repositories/IRoleRepository.cs
using AirTicketSystem.modules.role.Domain.aggregate;

namespace AirTicketSystem.modules.role.Domain.Repositories;

public interface IRoleRepository
{
    Task<Role?> FindByIdAsync(int id);
    Task<IReadOnlyCollection<Role>> FindAllAsync();
    Task<bool> ExistsByNombreAsync(string nombre);
    Task SaveAsync(Role role);
    Task UpdateAsync(Role role);
    Task DeleteAsync(int id);
}
