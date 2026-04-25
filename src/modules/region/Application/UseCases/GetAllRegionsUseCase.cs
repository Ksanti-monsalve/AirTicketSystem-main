// src/modules/region/Application/UseCases/GetAllRegionsUseCase.cs
using AirTicketSystem.modules.region.Domain.aggregate;
using AirTicketSystem.modules.region.Domain.Repositories;

namespace AirTicketSystem.modules.region.Application.UseCases;

public sealed class GetAllRegionsUseCase
{
    private readonly IRegionRepository _repository;

    public GetAllRegionsUseCase(IRegionRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyCollection<Region>> ExecuteAsync(
        CancellationToken cancellationToken = default)
    {
        return await _repository.FindAllAsync();
    }
}
