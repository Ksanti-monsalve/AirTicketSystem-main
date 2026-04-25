// src/modules/serviceclass/Domain/Repositories/IServiceClassRepository.cs
using AirTicketSystem.modules.serviceclass.Domain.aggregate;

namespace AirTicketSystem.modules.serviceclass.Domain.Repositories;

public interface IServiceClassRepository
{
    Task<ServiceClass?> FindByIdAsync(int id);
    Task<IReadOnlyCollection<ServiceClass>> FindAllAsync();
    Task<ServiceClass?> FindByCodigoAsync(string codigo);
    Task<bool> ExistsByCodigoAsync(string codigo);
    Task SaveAsync(ServiceClass serviceClass);
    Task UpdateAsync(ServiceClass serviceClass);
    Task DeleteAsync(int id);
}