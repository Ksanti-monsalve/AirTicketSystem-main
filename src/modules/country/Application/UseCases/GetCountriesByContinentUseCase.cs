// src/modules/country/Application/UseCases/GetCountriesByContinentUseCase.cs
using AirTicketSystem.modules.country.Domain.aggregate;
using AirTicketSystem.modules.country.Domain.Repositories;

namespace AirTicketSystem.modules.country.Application.UseCases;

public sealed class GetCountriesByContinentUseCase
{
    private readonly ICountryRepository _repository;

    public GetCountriesByContinentUseCase(ICountryRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyCollection<Country>> ExecuteAsync(
        int continenteId,
        CancellationToken cancellationToken = default)
    {
        if (continenteId <= 0)
            throw new ArgumentException("El ID del continente no es válido.");

        return await _repository.FindByContinenteAsync(continenteId);
    }
}
