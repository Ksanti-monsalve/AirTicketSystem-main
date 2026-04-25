// src/modules/airport/Application/UseCases/DeactivateAirportUseCase.cs
using AirTicketSystem.modules.airport.Domain.Repositories;

namespace AirTicketSystem.modules.airport.Application.UseCases;

public class DeactivateAirportUseCase
{
    private readonly IAirportRepository _repository;

    public DeactivateAirportUseCase(IAirportRepository repository)
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

        if (!aeropuerto.Activo.Valor)
            throw new InvalidOperationException(
                $"El aeropuerto '{aeropuerto.Nombre.Valor}' ya se encuentra inactivo.");

        aeropuerto.Desactivar();
        await _repository.UpdateAsync(aeropuerto);
    }
}