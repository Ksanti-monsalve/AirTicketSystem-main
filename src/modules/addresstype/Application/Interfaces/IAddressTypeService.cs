// src/modules/addresstype/Application/Interfaces/IAddressTypeService.cs
using AirTicketSystem.modules.addresstype.Domain.aggregate;

namespace AirTicketSystem.modules.addresstype.Application.Interfaces;

public interface IAddressTypeService
{
    Task<AddressType> CreateAsync(string descripcion);
    Task<AddressType> GetByIdAsync(int id);
    Task<IReadOnlyCollection<AddressType>> GetAllAsync();
    Task<AddressType> UpdateAsync(int id, string descripcion);
    Task DeleteAsync(int id);
}
