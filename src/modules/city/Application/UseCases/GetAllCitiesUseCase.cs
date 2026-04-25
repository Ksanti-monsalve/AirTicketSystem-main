// src/modules/city/Application/UseCases/GetAllCitiesUseCase.cs
using AirTicketSystem.modules.city.Domain.aggregate;
using AirTicketSystem.modules.city.Domain.Repositories;

namespace AirTicketSystem.modules.city.Application.UseCases;

public sealed class GetAllCitiesUseCase
{
    private readonly ICityRepository _repository;

    public GetAllCitiesUseCase(ICityRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyCollection<City>> ExecuteAsync(
        CancellationToken cancellationToken = default)
    {
        return await _repository.FindAllAsync();
    }
}
