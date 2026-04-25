// src/modules/aircraft/Application/UseCases/SendToMaintenanceUseCase.cs
using AirTicketSystem.modules.aircraft.Domain.Repositories;

namespace AirTicketSystem.modules.aircraft.Application.UseCases;

public class SendToMaintenanceUseCase
{
    private readonly IAircraftRepository _repository;

    public SendToMaintenanceUseCase(IAircraftRepository repository)
    {
        _repository = repository;
    }

    public async Task ExecuteAsync(
        int id,
        DateOnly fechaProximoMantenimiento,
        CancellationToken cancellationToken = default)
    {
        var avion = await _repository.FindByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un avión con ID {id}.");
        avion.EnviarAMantenimiento(fechaProximoMantenimiento);

        await _repository.UpdateAsync(avion);
    }
}