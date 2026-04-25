// src/modules/luggagerestriction/Application/Services/LuggageRestrictionService.cs
using AirTicketSystem.modules.luggagerestriction.Application.Interfaces;
using AirTicketSystem.modules.luggagerestriction.Application.UseCases;
using AirTicketSystem.modules.luggagerestriction.Domain.aggregate;

namespace AirTicketSystem.modules.luggagerestriction.Application.Services;

public sealed class LuggageRestrictionService : ILuggageRestrictionService
{
    private readonly CreateLuggageRestrictionUseCase _create;
    private readonly GetLuggageRestrictionByIdUseCase _getById;
    private readonly GetAllRestrictionsUseCase _getAll;
    private readonly GetRestrictionsByFareUseCase _getByFare;
    private readonly UpdateLuggageRestrictionUseCase _update;
    private readonly DeleteLuggageRestrictionUseCase _delete;

    public LuggageRestrictionService(
        CreateLuggageRestrictionUseCase create,
        GetLuggageRestrictionByIdUseCase getById,
        GetAllRestrictionsUseCase getAll,
        GetRestrictionsByFareUseCase getByFare,
        UpdateLuggageRestrictionUseCase update,
        DeleteLuggageRestrictionUseCase delete)
    {
        _create    = create;
        _getById   = getById;
        _getAll    = getAll;
        _getByFare = getByFare;
        _update    = update;
        _delete    = delete;
    }

    public Task<LuggageRestriction> CreateAsync(
        int tarifaId, int tipoEquipajeId,
        int piezasIncluidas, decimal pesoMaximoKg, decimal costoExcesoKg,
        int? largoMaxCm, int? anchoMaxCm, int? altoMaxCm)
        => _create.ExecuteAsync(
            tarifaId, tipoEquipajeId,
            piezasIncluidas, pesoMaximoKg, costoExcesoKg,
            largoMaxCm, anchoMaxCm, altoMaxCm);

    public Task<LuggageRestriction> GetByIdAsync(int id)
        => _getById.ExecuteAsync(id);

    public Task<IReadOnlyCollection<LuggageRestriction>> GetAllAsync()
        => _getAll.ExecuteAsync();

    public Task<IReadOnlyCollection<LuggageRestriction>> GetByFareAsync(int tarifaId)
        => _getByFare.ExecuteAsync(tarifaId);

    public Task<LuggageRestriction> UpdateAsync(
        int id, int piezasIncluidas,
        decimal pesoMaximoKg, decimal costoExcesoKg,
        int? largoMaxCm, int? anchoMaxCm, int? altoMaxCm)
        => _update.ExecuteAsync(
            id, piezasIncluidas,
            pesoMaximoKg, costoExcesoKg,
            largoMaxCm, anchoMaxCm, altoMaxCm);

    public Task DeleteAsync(int id) => _delete.ExecuteAsync(id);
}
