// src/modules/aircraftmodel/Application/UseCases/UpdateAircraftModelUseCase.cs
using AirTicketSystem.modules.aircraftmodel.Domain.Repositories;
using AirTicketSystem.modules.aircraftmodel.Domain.aggregate;

namespace AirTicketSystem.modules.aircraftmodel.Application.UseCases;

public class UpdateAircraftModelUseCase
{
    private readonly IAircraftModelRepository _repository;

    public UpdateAircraftModelUseCase(IAircraftModelRepository repository)
    {
        _repository = repository;
    }

    public async Task<AircraftModel> ExecuteAsync(
        int id,
        string nombre,
        int? autonomiKm,
        int? velocidadKmh,
        string? descripcion,
        CancellationToken cancellationToken = default)
    {
        var modelo = await _repository.FindByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un modelo de avión con ID {id}.");

        modelo.ActualizarNombre(nombre);
        modelo.ActualizarEspecificaciones(autonomiKm, velocidadKmh);
        modelo.ActualizarDescripcion(descripcion);

        await _repository.UpdateAsync(modelo);
        return modelo;
    }
}