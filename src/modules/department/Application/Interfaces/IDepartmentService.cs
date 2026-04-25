// src/modules/department/Application/Interfaces/IDepartmentService.cs
using AirTicketSystem.modules.department.Domain.aggregate;

namespace AirTicketSystem.modules.department.Application.Interfaces;

public interface IDepartmentService
{
    Task<Department> CreateAsync(int regionId, string nombre, string? codigo);
    Task<Department> GetByIdAsync(int id);
    Task<IReadOnlyCollection<Department>> GetAllAsync();
    Task<IReadOnlyCollection<Department>> GetByRegionAsync(int regionId);
    Task<Department> UpdateAsync(int id, string nombre, string? codigo);
    Task DeleteAsync(int id);
}
