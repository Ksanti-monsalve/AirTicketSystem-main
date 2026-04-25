// src/modules/country/Application/UseCases/GetCountryByIdUseCase.cs
using AirTicketSystem.modules.country.Domain.aggregate;
using AirTicketSystem.modules.country.Domain.Repositories;

namespace AirTicketSystem.modules.country.Application.UseCases;

public sealed class GetCountryByIdUseCase
{
    private readonly ICountryRepository _repository;

    public GetCountryByIdUseCase(ICountryRepository repository)
    {
        _repository = repository;
    }

    public async Task<Country> ExecuteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        return await _repository.FindByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un país con ID {id}.");
    }
}
