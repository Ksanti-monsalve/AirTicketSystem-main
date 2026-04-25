// src/modules/luggagetype/Application/Interfaces/ILuggageTypeService.cs
using AirTicketSystem.modules.luggagetype.Domain.aggregate;

namespace AirTicketSystem.modules.luggagetype.Application.Interfaces;

public interface ILuggageTypeService
{
    Task<LuggageType> CreateAsync(string nombre);
    Task<LuggageType> GetByIdAsync(int id);
    Task<IReadOnlyCollection<LuggageType>> GetAllAsync();
    Task<LuggageType> UpdateAsync(int id, string nombre);
    Task DeleteAsync(int id);
}
