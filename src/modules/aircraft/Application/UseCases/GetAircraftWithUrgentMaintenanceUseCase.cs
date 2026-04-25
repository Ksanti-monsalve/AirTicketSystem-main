// src/modules/aircraft/Application/UseCases/GetAircraftWithUrgentMaintenanceUseCase.cs
using AirTicketSystem.modules.aircraft.Domain.Repositories;
using AirTicketSystem.modules.aircraft.Domain.aggregate;

namespace AirTicketSystem.modules.aircraft.Application.UseCases;

public class GetAircraftWithUrgentMaintenanceUseCase
{
    private readonly IAircraftRepository _repository;

    public GetAircraftWithUrgentMaintenanceUseCase(IAircraftRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyCollection<Aircraft>> ExecuteAsync(
        CancellationToken cancellationToken = default)
        => await _repository.FindConMantenimientoUrgenteAsync();
}