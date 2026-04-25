// src/modules/luggagerestriction/Domain/Repositories/ILuggageRestrictionRepository.cs
using AirTicketSystem.modules.luggagerestriction.Domain.aggregate;

namespace AirTicketSystem.modules.luggagerestriction.Domain.Repositories;

public interface ILuggageRestrictionRepository
{
    Task<LuggageRestriction?> FindByIdAsync(int id);
    Task<IReadOnlyCollection<LuggageRestriction>> FindAllAsync();
    Task<IReadOnlyCollection<LuggageRestriction>> FindByTarifaAsync(int tarifaId);
    Task<LuggageRestriction?> FindByTarifaAndTipoEquipajeAsync(int tarifaId, int tipoEquipajeId);
    Task<bool> ExistsByTarifaAndTipoEquipajeAsync(int tarifaId, int tipoEquipajeId);
    Task SaveAsync(LuggageRestriction restriction);
    Task UpdateAsync(LuggageRestriction restriction);
    Task DeleteAsync(int id);
}
