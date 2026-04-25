// src/modules/pilotrating/Application/UseCases/RenewPilotRatingUseCase.cs
using AirTicketSystem.modules.pilotrating.Domain.aggregate;
using AirTicketSystem.modules.pilotrating.Domain.Repositories;

namespace AirTicketSystem.modules.pilotrating.Application.UseCases;

public sealed class RenewPilotRatingUseCase
{
    private readonly IPilotRatingRepository _repository;

    public RenewPilotRatingUseCase(IPilotRatingRepository repository)
        => _repository = repository;

    public async Task<PilotRating> ExecuteAsync(
        int id, DateOnly nuevaFechaVencimiento, CancellationToken cancellationToken = default)
    {
        var habilitacion = await _repository.FindByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró una habilitación con ID {id}.");

        habilitacion.Renovar(nuevaFechaVencimiento);
        await _repository.UpdateAsync(habilitacion);
        return habilitacion;
    }
}
