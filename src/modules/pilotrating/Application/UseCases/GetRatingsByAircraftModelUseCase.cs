// src/modules/pilotrating/Application/UseCases/GetRatingsByAircraftModelUseCase.cs
using AirTicketSystem.modules.pilotrating.Domain.aggregate;
using AirTicketSystem.modules.pilotrating.Domain.Repositories;

namespace AirTicketSystem.modules.pilotrating.Application.UseCases;

public sealed class GetRatingsByAircraftModelUseCase
{
    private readonly IPilotRatingRepository _repository;

    public GetRatingsByAircraftModelUseCase(IPilotRatingRepository repository)
        => _repository = repository;

    public async Task<IReadOnlyCollection<PilotRating>> ExecuteAsync(
        int modeloAvionId, CancellationToken cancellationToken = default)
    {
        if (modeloAvionId <= 0)
            throw new ArgumentException("El ID del modelo de avión no es válido.");

        return await _repository.FindByModeloAvionAsync(modeloAvionId);
    }
}
