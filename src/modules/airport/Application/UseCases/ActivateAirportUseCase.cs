// src/modules/airport/Application/UseCases/ActivateAirportUseCase.cs
using AirTicketSystem.modules.airport.Domain.Repositories;

namespace AirTicketSystem.modules.airport.Application.UseCases;

public class ActivateAirportUseCase
{
    private readonly IAirportRepository _repository;

    public ActivateAirportUseCase(IAirportRepository repository)
    {
        _repository = repository;
    }

    public async Task ExecuteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var aeropuerto = await _repository.FindByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un aeropuerto con ID {id}.");

        if (aeropuerto.Activo.Valor)
            throw new InvalidOperationException(
                $"El aeropuerto '{aeropuerto.Nombre.Valor}' ya se encuentra activo.");

        aeropuerto.Activar();
        await _repository.UpdateAsync(aeropuerto);
    }
}