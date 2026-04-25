// src/modules/airline/Application/UseCases/ActivateAirlineUseCase.cs
using AirTicketSystem.modules.airline.Domain.Repositories;

namespace AirTicketSystem.modules.airline.Application.UseCases;

public class ActivateAirlineUseCase
{
    private readonly IAirlineRepository _repository;

    public ActivateAirlineUseCase(IAirlineRepository repository)
    {
        _repository = repository;
    }

    public async Task ExecuteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var aerolinea = await _repository.FindByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró una aerolínea con ID {id}.");

        aerolinea.Activar();
        await _repository.UpdateAsync(aerolinea);
    }
}