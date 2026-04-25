// src/modules/role/Application/Interfaces/IRoleService.cs
using AirTicketSystem.modules.role.Domain.aggregate;

namespace AirTicketSystem.modules.role.Application.Interfaces;

public interface IRoleService
{
    Task<Role> CreateAsync(string nombre);
    Task<Role> GetByIdAsync(int id);
    Task<IReadOnlyCollection<Role>> GetAllAsync();
    Task<Role> UpdateAsync(int id, string nombre);
    Task DeleteAsync(int id);
}
