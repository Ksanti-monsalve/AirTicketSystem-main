// src/modules/flightcrew/Application/Services/FlightCrewService.cs
using AirTicketSystem.modules.flightcrew.Application.Interfaces;
using AirTicketSystem.modules.flightcrew.Application.UseCases;
using AirTicketSystem.modules.flightcrew.Domain.aggregate;

namespace AirTicketSystem.modules.flightcrew.Application.Services;

public sealed class FlightCrewService : IFlightCrewService
{
    private readonly AssignCrewMemberUseCase _assign;
    private readonly GetCrewByFlightUseCase _getByFlight;
    private readonly ValidateFlightCrewUseCase _validate;
    private readonly RemoveCrewMemberUseCase _remove;
    private readonly DeleteFlightCrewUseCase _delete;

    public FlightCrewService(
        AssignCrewMemberUseCase assign,
        GetCrewByFlightUseCase getByFlight,
        ValidateFlightCrewUseCase validate,
        RemoveCrewMemberUseCase remove,
        DeleteFlightCrewUseCase delete)
    {
        _assign     = assign;
        _getByFlight = getByFlight;
        _validate   = validate;
        _remove     = remove;
        _delete     = delete;
    }

    public Task<FlightCrew> AssignCrewMemberAsync(
        int vueloId, int trabajadorId, string rolEnVuelo)
        => _assign.ExecuteAsync(vueloId, trabajadorId, rolEnVuelo);

    public Task<IReadOnlyCollection<FlightCrew>> GetByFlightAsync(int vueloId)
        => _getByFlight.ExecuteAsync(vueloId);

    public Task<bool> ValidateCrewAsync(int vueloId)
        => _validate.ExecuteAsync(vueloId);

    public Task RemoveCrewMemberAsync(int flightCrewId)
        => _remove.ExecuteAsync(flightCrewId);

    public Task DeleteAsync(int id)
        => _delete.ExecuteAsync(id);
}