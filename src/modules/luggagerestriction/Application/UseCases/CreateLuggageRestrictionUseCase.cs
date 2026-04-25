// src/modules/luggagerestriction/Application/UseCases/CreateLuggageRestrictionUseCase.cs
using AirTicketSystem.modules.luggagerestriction.Domain.aggregate;
using AirTicketSystem.modules.luggagerestriction.Domain.Repositories;
using AirTicketSystem.modules.fare.Domain.Repositories;
using AirTicketSystem.modules.luggagetype.Domain.Repositories;

namespace AirTicketSystem.modules.luggagerestriction.Application.UseCases;

public sealed class CreateLuggageRestrictionUseCase
{
    private readonly ILuggageRestrictionRepository _repository;
    private readonly IFareRepository _fareRepository;
    private readonly ILuggageTypeRepository _luggageTypeRepository;

    public CreateLuggageRestrictionUseCase(
        ILuggageRestrictionRepository repository,
        IFareRepository fareRepository,
        ILuggageTypeRepository luggageTypeRepository)
    {
        _repository           = repository;
        _fareRepository       = fareRepository;
        _luggageTypeRepository = luggageTypeRepository;
    }

    public async Task<LuggageRestriction> ExecuteAsync(
        int tarifaId,
        int tipoEquipajeId,
        int piezasIncluidas,
        decimal pesoMaximoKg,
        decimal costoExcesoKg,
        int? largoMaxCm = null,
        int? anchoMaxCm = null,
        int? altoMaxCm = null,
        CancellationToken cancellationToken = default)
    {
        _ = await _fareRepository.FindByIdAsync(tarifaId)
            ?? throw new KeyNotFoundException(
                $"No se encontró una tarifa con ID {tarifaId}.");

        _ = await _luggageTypeRepository.FindByIdAsync(tipoEquipajeId)
            ?? throw new KeyNotFoundException(
                $"No se encontró un tipo de equipaje con ID {tipoEquipajeId}.");

        if (await _repository.ExistsByTarifaAndTipoEquipajeAsync(tarifaId, tipoEquipajeId))
            throw new InvalidOperationException(
                "Ya existe una restricción de equipaje para esta tarifa y tipo de equipaje.");

        var restriction = LuggageRestriction.Crear(
            tarifaId, tipoEquipajeId,
            piezasIncluidas, pesoMaximoKg, costoExcesoKg,
            largoMaxCm, anchoMaxCm, altoMaxCm);

        await _repository.SaveAsync(restriction);
        return restriction;
    }
}
