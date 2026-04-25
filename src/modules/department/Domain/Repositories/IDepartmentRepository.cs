// src/modules/department/Domain/Repositories/IDepartmentRepository.cs
using AirTicketSystem.modules.department.Domain.aggregate;

namespace AirTicketSystem.modules.department.Domain.Repositories;

public interface IDepartmentRepository
{
    Task<Department?> FindByIdAsync(int id);
    Task<IReadOnlyCollection<Department>> FindAllAsync();
    Task<IReadOnlyCollection<Department>> FindByRegionAsync(int regionId);
    Task<bool> ExistsByNombreAndRegionAsync(string nombre, int regionId);
    Task SaveAsync(Department department);
    Task UpdateAsync(Department department);
    Task DeleteAsync(int id);
}
