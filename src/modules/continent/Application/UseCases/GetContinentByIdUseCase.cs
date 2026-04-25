// src/modules/continent/Application/UseCases/GetContinentByIdUseCase.cs
using AirTicketSystem.modules.continent.Domain.aggregate;
using AirTicketSystem.modules.continent.Domain.Repositories;

namespace AirTicketSystem.modules.continent.Application.UseCases;

public sealed class GetContinentByIdUseCase
{
    private readonly IContinentRepository _repository;

    public GetContinentByIdUseCase(IContinentRepository repository)
    {
        _repository = repository;
    }

    public async Task<Continent> ExecuteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        if (id <= 0)
            throw new ArgumentException("El ID del continente no es válido.");

        return await _repository.FindByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un continente con ID {id}.");
    }
}
