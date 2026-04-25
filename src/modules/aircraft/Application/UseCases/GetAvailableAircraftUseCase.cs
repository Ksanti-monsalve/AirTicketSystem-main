// src/modules/aircraft/Application/UseCases/GetAvailableAircraftUseCase.cs
using AirTicketSystem.modules.aircraft.Domain.Repositories;
using AirTicketSystem.modules.aircraft.Domain.aggregate;

namespace AirTicketSystem.modules.aircraft.Application.UseCases;

public class GetAvailableAircraftUseCase
{
    private readonly IAircraftRepository _repository;

    public GetAvailableAircraftUseCase(IAircraftRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyCollection<Aircraft>> ExecuteAsync(
        CancellationToken cancellationToken = default)
        => await _repository.FindDisponiblesAsync();
}