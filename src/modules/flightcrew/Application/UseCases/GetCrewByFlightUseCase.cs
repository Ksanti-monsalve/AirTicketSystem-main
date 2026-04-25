// src/modules/flightcrew/Application/UseCases/GetCrewByFlightUseCase.cs
using AirTicketSystem.modules.flightcrew.Domain.Repositories;
using AirTicketSystem.modules.flightcrew.Domain.aggregate;

namespace AirTicketSystem.modules.flightcrew.Application.UseCases;

public sealed class GetCrewByFlightUseCase
{
    private readonly IFlightCrewRepository _repository;

    public GetCrewByFlightUseCase(IFlightCrewRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyCollection<FlightCrew>> ExecuteAsync(
        int vueloId,
        CancellationToken cancellationToken = default)
    {
        if (vueloId <= 0)
            throw new ArgumentException("El ID del vuelo no es válido.");

        return await _repository.FindByVueloAsync(vueloId);
    }
}