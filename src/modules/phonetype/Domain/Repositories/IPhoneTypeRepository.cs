// src/modules/phonetype/Domain/Repositories/IPhoneTypeRepository.cs
using AirTicketSystem.modules.phonetype.Domain.aggregate;

namespace AirTicketSystem.modules.phonetype.Domain.Repositories;

public interface IPhoneTypeRepository
{
    Task<PhoneType?> FindByIdAsync(int id);
    Task<IReadOnlyCollection<PhoneType>> FindAllAsync();
    Task<bool> ExistsByDescripcionAsync(string descripcion);
    Task SaveAsync(PhoneType phoneType);
    Task UpdateAsync(PhoneType phoneType);
    Task DeleteAsync(int id);
}
