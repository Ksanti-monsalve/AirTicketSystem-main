// src/modules/city/Application/UseCases/GetCityByIdUseCase.cs
using AirTicketSystem.modules.city.Domain.aggregate;
using AirTicketSystem.modules.city.Domain.Repositories;

namespace AirTicketSystem.modules.city.Application.UseCases;

public sealed class GetCityByIdUseCase
{
    private readonly ICityRepository _repository;

    public GetCityByIdUseCase(ICityRepository repository)
    {
        _repository = repository;
    }

    public async Task<City> ExecuteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        return await _repository.FindByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró una ciudad con ID {id}.");
    }
}
