// src/modules/aircraft/Application/UseCases/UpdateAircraftUseCase.cs
using AirTicketSystem.modules.aircraft.Domain.Repositories;
using AirTicketSystem.modules.aircraft.Domain.aggregate;

namespace AirTicketSystem.modules.aircraft.Application.UseCases;

public class UpdateAircraftUseCase
{
    private readonly IAircraftRepository _repository;

    public UpdateAircraftUseCase(IAircraftRepository repository)
    {
        _repository = repository;
    }

    public async Task<Aircraft> ExecuteAsync(
        int id,
        DateOnly? fechaFabricacion,
        DateOnly? fechaProximoMantenimiento,
        CancellationToken cancellationToken = default)
    {
        var avion = await _repository.FindByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un avión con ID {id}.");

        avion.ActualizarFechas(fechaFabricacion, fechaProximoMantenimiento);

        await _repository.UpdateAsync(avion);
        return avion;
    }
}