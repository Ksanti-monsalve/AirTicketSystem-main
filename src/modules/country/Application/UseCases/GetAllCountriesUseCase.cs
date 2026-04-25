// src/modules/country/Application/UseCases/GetAllCountriesUseCase.cs
using AirTicketSystem.modules.country.Domain.aggregate;
using AirTicketSystem.modules.country.Domain.Repositories;

namespace AirTicketSystem.modules.country.Application.UseCases;

public sealed class GetAllCountriesUseCase
{
    private readonly ICountryRepository _repository;

    public GetAllCountriesUseCase(ICountryRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyCollection<Country>> ExecuteAsync(
        CancellationToken cancellationToken = default)
    {
        return await _repository.FindAllAsync();
    }
}
