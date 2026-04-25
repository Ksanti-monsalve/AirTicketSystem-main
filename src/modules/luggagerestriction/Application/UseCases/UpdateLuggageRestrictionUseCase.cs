// src/modules/luggagerestriction/Application/UseCases/UpdateLuggageRestrictionUseCase.cs
using AirTicketSystem.modules.luggagerestriction.Domain.aggregate;
using AirTicketSystem.modules.luggagerestriction.Domain.Repositories;

namespace AirTicketSystem.modules.luggagerestriction.Application.UseCases;

public sealed class UpdateLuggageRestrictionUseCase
{
    private readonly ILuggageRestrictionRepository _repository;

    public UpdateLuggageRestrictionUseCase(ILuggageRestrictionRepository repository)
        => _repository = repository;

    public async Task<LuggageRestriction> ExecuteAsync(
        int id,
        int piezasIncluidas,
        decimal pesoMaximoKg,
        decimal costoExcesoKg,
        int? largoMaxCm,
        int? anchoMaxCm,
        int? altoMaxCm,
        CancellationToken cancellationToken = default)
    {
        var restriction = await _repository.FindByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró una restricción de equipaje con ID {id}.");

        restriction.ActualizarPiezasIncluidas(piezasIncluidas);
        restriction.ActualizarPeso(pesoMaximoKg, costoExcesoKg);
        restriction.ActualizarDimensiones(largoMaxCm, anchoMaxCm, altoMaxCm);
        await _repository.UpdateAsync(restriction);
        return restriction;
    }
}
