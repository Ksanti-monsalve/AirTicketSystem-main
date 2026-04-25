// src/modules/luggagetype/Domain/Repositories/ILuggageTypeRepository.cs
using AirTicketSystem.modules.luggagetype.Domain.aggregate;

namespace AirTicketSystem.modules.luggagetype.Domain.Repositories;

public interface ILuggageTypeRepository
{
    Task<LuggageType?> FindByIdAsync(int id);
    Task<IReadOnlyCollection<LuggageType>> FindAllAsync();
    Task<bool> ExistsByNombreAsync(string nombre);
    Task SaveAsync(LuggageType luggageType);
    Task UpdateAsync(LuggageType luggageType);
    Task DeleteAsync(int id);
}
