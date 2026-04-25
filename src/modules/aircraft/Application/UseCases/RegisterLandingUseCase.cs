// src/modules/aircraft/Application/UseCases/RegisterLandingUseCase.cs
using AirTicketSystem.modules.aircraft.Domain.Repositories;

namespace AirTicketSystem.modules.aircraft.Application.UseCases;

public class RegisterLandingUseCase
{
    private readonly IAircraftRepository _repository;

    public RegisterLandingUseCase(IAircraftRepository repository)
    {
        _repository = repository;
    }

    public async Task ExecuteAsync(
        int id,
        decimal horasVuelo,
        CancellationToken cancellationToken = default)
    {
        var avion = await _repository.FindByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un avión con ID {id}.");
        avion.RegistrarAterrizaje(horasVuelo);

        await _repository.UpdateAsync(avion);
    }
}