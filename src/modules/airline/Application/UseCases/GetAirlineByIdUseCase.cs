// src/modules/airline/Application/UseCases/GetAirlineByIdUseCase.cs
using AirTicketSystem.modules.airline.Domain.Repositories;
using AirTicketSystem.modules.airline.Domain.aggregate;

namespace AirTicketSystem.modules.airline.Application.UseCases;

public class GetAirlineByIdUseCase
{
    private readonly IAirlineRepository _repository;

    public GetAirlineByIdUseCase(IAirlineRepository repository)
    {
        _repository = repository;
    }

    public async Task<Airline> ExecuteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        if (id <= 0)
            throw new ArgumentException("El ID de la aerolínea no es válido.");

        return await _repository.FindByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró una aerolínea con ID {id}.");
    }
}