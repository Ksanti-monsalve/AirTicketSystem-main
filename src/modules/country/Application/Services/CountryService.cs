// src/modules/country/Application/Services/CountryService.cs
using AirTicketSystem.modules.country.Application.Interfaces;
using AirTicketSystem.modules.country.Application.UseCases;
using AirTicketSystem.modules.country.Domain.aggregate;

namespace AirTicketSystem.modules.country.Application.Services;

public sealed class CountryService : ICountryService
{
    private readonly CreateCountryUseCase _create;
    private readonly GetCountryByIdUseCase _getById;
    private readonly GetAllCountriesUseCase _getAll;
    private readonly GetCountriesByContinentUseCase _getByContinent;
    private readonly UpdateCountryUseCase _update;
    private readonly DeleteCountryUseCase _delete;

    public CountryService(
        CreateCountryUseCase create,
        GetCountryByIdUseCase getById,
        GetAllCountriesUseCase getAll,
        GetCountriesByContinentUseCase getByContinent,
        UpdateCountryUseCase update,
        DeleteCountryUseCase delete)
    {
        _create         = create;
        _getById        = getById;
        _getAll         = getAll;
        _getByContinent = getByContinent;
        _update         = update;
        _delete         = delete;
    }

    public Task<Country> CreateAsync(
        int continenteId, string nombre,
        string codigoIso2, string codigoIso3)
        => _create.ExecuteAsync(continenteId, nombre, codigoIso2, codigoIso3);

    public Task<Country> GetByIdAsync(int id)
        => _getById.ExecuteAsync(id);

    public Task<IReadOnlyCollection<Country>> GetAllAsync()
        => _getAll.ExecuteAsync();

    public Task<IReadOnlyCollection<Country>> GetByContinentAsync(int continenteId)
        => _getByContinent.ExecuteAsync(continenteId);

    public Task<Country> UpdateAsync(
        int id, string nombre,
        string codigoIso2, string codigoIso3)
        => _update.ExecuteAsync(id, nombre, codigoIso2, codigoIso3);

    public Task DeleteAsync(int id)
        => _delete.ExecuteAsync(id);
}
