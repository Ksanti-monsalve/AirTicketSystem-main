// src/modules/airline/Application/UseCases/DeactivateAirlineUseCase.cs
using AirTicketSystem.modules.airline.Domain.Repositories;

namespace AirTicketSystem.modules.airline.Application.UseCases;

public class DeactivateAirlineUseCase
{
    private readonly IAirlineRepository _repository;

    public DeactivateAirlineUseCase(IAirlineRepository repository)
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

        aerolinea.Desactivar();
        await _repository.UpdateAsync(aerolinea);
    }
}