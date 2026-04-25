// src/modules/luggagerestriction/Application/Interfaces/ILuggageRestrictionService.cs
using AirTicketSystem.modules.luggagerestriction.Domain.aggregate;

namespace AirTicketSystem.modules.luggagerestriction.Application.Interfaces;

public interface ILuggageRestrictionService
{
    Task<LuggageRestriction> CreateAsync(
        int tarifaId, int tipoEquipajeId,
        int piezasIncluidas, decimal pesoMaximoKg, decimal costoExcesoKg,
        int? largoMaxCm, int? anchoMaxCm, int? altoMaxCm);
    Task<LuggageRestriction> GetByIdAsync(int id);
    Task<IReadOnlyCollection<LuggageRestriction>> GetAllAsync();
    Task<IReadOnlyCollection<LuggageRestriction>> GetByFareAsync(int tarifaId);
    Task<LuggageRestriction> UpdateAsync(
        int id, int piezasIncluidas,
        decimal pesoMaximoKg, decimal costoExcesoKg,
        int? largoMaxCm, int? anchoMaxCm, int? altoMaxCm);
    Task DeleteAsync(int id);
}
