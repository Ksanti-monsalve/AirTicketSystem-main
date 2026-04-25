// src/modules/region/Application/UseCases/GetRegionByIdUseCase.cs
using AirTicketSystem.modules.region.Domain.aggregate;
using AirTicketSystem.modules.region.Domain.Repositories;

namespace AirTicketSystem.modules.region.Application.UseCases;

public sealed class GetRegionByIdUseCase
{
    private readonly IRegionRepository _repository;

    public GetRegionByIdUseCase(IRegionRepository repository)
    {
        _repository = repository;
    }

    public async Task<Region> ExecuteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        return await _repository.FindByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró una región con ID {id}.");
    }
}
