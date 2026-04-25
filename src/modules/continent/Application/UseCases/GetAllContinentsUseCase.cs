// src/modules/continent/Application/UseCases/GetAllContinentsUseCase.cs
using AirTicketSystem.modules.continent.Domain.aggregate;
using AirTicketSystem.modules.continent.Domain.Repositories;

namespace AirTicketSystem.modules.continent.Application.UseCases;

public sealed class GetAllContinentsUseCase
{
    private readonly IContinentRepository _repository;

    public GetAllContinentsUseCase(IContinentRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyCollection<Continent>> ExecuteAsync(
        CancellationToken cancellationToken = default)
    {
        return await _repository.FindAllAsync();
    }
}
