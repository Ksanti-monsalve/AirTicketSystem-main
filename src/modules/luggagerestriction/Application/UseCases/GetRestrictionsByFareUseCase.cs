// src/modules/luggagerestriction/Application/UseCases/GetRestrictionsByFareUseCase.cs
using AirTicketSystem.modules.luggagerestriction.Domain.aggregate;
using AirTicketSystem.modules.luggagerestriction.Domain.Repositories;

namespace AirTicketSystem.modules.luggagerestriction.Application.UseCases;

public sealed class GetRestrictionsByFareUseCase
{
    private readonly ILuggageRestrictionRepository _repository;

    public GetRestrictionsByFareUseCase(ILuggageRestrictionRepository repository)
        => _repository = repository;

    public async Task<IReadOnlyCollection<LuggageRestriction>> ExecuteAsync(
        int tarifaId, CancellationToken cancellationToken = default)
    {
        if (tarifaId <= 0)
            throw new ArgumentException("El ID de la tarifa no es válido.");

        return await _repository.FindByTarifaAsync(tarifaId);
    }
}
