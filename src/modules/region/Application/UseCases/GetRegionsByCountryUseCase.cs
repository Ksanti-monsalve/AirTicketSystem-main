// src/modules/region/Application/UseCases/GetRegionsByCountryUseCase.cs
using AirTicketSystem.modules.region.Domain.aggregate;
using AirTicketSystem.modules.region.Domain.Repositories;

namespace AirTicketSystem.modules.region.Application.UseCases;

public sealed class GetRegionsByCountryUseCase
{
    private readonly IRegionRepository _repository;

    public GetRegionsByCountryUseCase(IRegionRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyCollection<Region>> ExecuteAsync(
        int paisId,
        CancellationToken cancellationToken = default)
    {
        if (paisId <= 0)
            throw new ArgumentException("El ID del país no es válido.");

        return await _repository.FindByPaisAsync(paisId);
    }
}
