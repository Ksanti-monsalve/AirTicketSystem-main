// src/modules/pilotrating/Application/UseCases/GetViigenteRatingsUseCase.cs
using AirTicketSystem.modules.pilotrating.Domain.aggregate;
using AirTicketSystem.modules.pilotrating.Domain.Repositories;

namespace AirTicketSystem.modules.pilotrating.Application.UseCases;

public sealed class GetVigenteRatingsUseCase
{
    private readonly IPilotRatingRepository _repository;

    public GetVigenteRatingsUseCase(IPilotRatingRepository repository)
        => _repository = repository;

    public Task<IReadOnlyCollection<PilotRating>> ExecuteAsync(
        CancellationToken cancellationToken = default)
        => _repository.FindVigentesAsync();
}
