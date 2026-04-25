// src/modules/pilotrating/Application/UseCases/GetPilotRatingByIdUseCase.cs
using AirTicketSystem.modules.pilotrating.Domain.aggregate;
using AirTicketSystem.modules.pilotrating.Domain.Repositories;

namespace AirTicketSystem.modules.pilotrating.Application.UseCases;

public sealed class GetPilotRatingByIdUseCase
{
    private readonly IPilotRatingRepository _repository;

    public GetPilotRatingByIdUseCase(IPilotRatingRepository repository)
        => _repository = repository;

    public async Task<PilotRating> ExecuteAsync(int id, CancellationToken cancellationToken = default)
    {
        if (id <= 0)
            throw new ArgumentException("El ID de la habilitación no es válido.");

        return await _repository.FindByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró una habilitación con ID {id}.");
    }
}
