// src/modules/aircraft/Application/UseCases/RegisterMaintenanceUseCase.cs
using AirTicketSystem.modules.aircraft.Domain.Repositories;

namespace AirTicketSystem.modules.aircraft.Application.UseCases;

public class RegisterMaintenanceUseCase
{
    private readonly IAircraftRepository _repository;

    public RegisterMaintenanceUseCase(IAircraftRepository repository)
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
        avion.RegistrarMantenimiento(fechaProximoMantenimiento);

        await _repository.UpdateAsync(avion);
    }
}