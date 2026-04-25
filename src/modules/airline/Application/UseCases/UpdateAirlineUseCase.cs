// src/modules/airline/Application/UseCases/UpdateAirlineUseCase.cs
using AirTicketSystem.modules.airline.Domain.Repositories;
using AirTicketSystem.modules.airline.Domain.aggregate;

namespace AirTicketSystem.modules.airline.Application.UseCases;

public class UpdateAirlineUseCase
{
    private readonly IAirlineRepository _repository;

    public UpdateAirlineUseCase(IAirlineRepository repository)
    {
        _repository = repository;
    }

    public async Task<Airline> ExecuteAsync(
        int id,
        string nombre,
        string? nombreComercial,
        string? sitioWeb,
        CancellationToken cancellationToken = default)
    {
        var aerolinea = await _repository.FindByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró una aerolínea con ID {id}.");

        aerolinea.ActualizarNombre(nombre, nombreComercial);
        aerolinea.ActualizarSitioWeb(sitioWeb);

        await _repository.UpdateAsync(aerolinea);
        return aerolinea;
    }
}