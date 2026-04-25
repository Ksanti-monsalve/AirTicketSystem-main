// src/modules/airport/Application/UseCases/UpdateAirportUseCase.cs
using AirTicketSystem.modules.airport.Domain.Repositories;
using AirTicketSystem.modules.airport.Domain.aggregate;

namespace AirTicketSystem.modules.airport.Application.UseCases;

public class UpdateAirportUseCase
{
    private readonly IAirportRepository _repository;

    public UpdateAirportUseCase(IAirportRepository repository)
    {
        _repository = repository;
    }

    public async Task<Airport> ExecuteAsync(
        int id,
        string nombre,
        string? direccion,
        CancellationToken cancellationToken = default)
    {
        var aeropuerto = await _repository.FindByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un aeropuerto con ID {id}.");

        aeropuerto.ActualizarNombre(nombre);
        aeropuerto.ActualizarDireccion(direccion);

        await _repository.UpdateAsync(aeropuerto);
        return aeropuerto;
    }
}