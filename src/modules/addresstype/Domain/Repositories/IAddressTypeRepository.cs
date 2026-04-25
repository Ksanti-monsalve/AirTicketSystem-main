// src/modules/addresstype/Domain/Repositories/IAddressTypeRepository.cs
using AirTicketSystem.modules.addresstype.Domain.aggregate;

namespace AirTicketSystem.modules.addresstype.Domain.Repositories;

public interface IAddressTypeRepository
{
    Task<AddressType?> FindByIdAsync(int id);
    Task<IReadOnlyCollection<AddressType>> FindAllAsync();
    Task<bool> ExistsByDescripcionAsync(string descripcion);
    Task SaveAsync(AddressType addressType);
    Task UpdateAsync(AddressType addressType);
    Task DeleteAsync(int id);
}
