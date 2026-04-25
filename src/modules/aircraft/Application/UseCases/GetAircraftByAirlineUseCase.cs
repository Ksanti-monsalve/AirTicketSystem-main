// src/modules/aircraft/Application/UseCases/GetAircraftByAirlineUseCase.cs
using AirTicketSystem.modules.aircraft.Domain.Repositories;
using AirTicketSystem.modules.aircraft.Domain.aggregate;

namespace AirTicketSystem.modules.aircraft.Application.UseCases;

public class GetAircraftByAirlineUseCase
{
    private readonly IAircraftRepository _repository;

    public GetAircraftByAirlineUseCase(IAircraftRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyCollection<Aircraft>> ExecuteAsync(
        int aerolineaId,
        CancellationToken cancellationToken = default)
    {
        if (aerolineaId <= 0)
            throw new ArgumentException("El ID de la aerolínea no es válido.");

        return await _repository.FindByAerolineaAsync(aerolineaId);
    }
}