// src/modules/phonetype/Application/Interfaces/IPhoneTypeService.cs
using AirTicketSystem.modules.phonetype.Domain.aggregate;

namespace AirTicketSystem.modules.phonetype.Application.Interfaces;

public interface IPhoneTypeService
{
    Task<PhoneType> CreateAsync(string descripcion);
    Task<PhoneType> GetByIdAsync(int id);
    Task<IReadOnlyCollection<PhoneType>> GetAllAsync();
    Task<PhoneType> UpdateAsync(int id, string descripcion);
    Task DeleteAsync(int id);
}
