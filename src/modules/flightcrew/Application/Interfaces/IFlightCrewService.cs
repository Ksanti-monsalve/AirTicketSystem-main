// src/modules/flightcrew/Application/Interfaces/IFlightCrewService.cs
using AirTicketSystem.modules.flightcrew.Domain.aggregate;

namespace AirTicketSystem.modules.flightcrew.Application.Interfaces;

public interface IFlightCrewService
{
    Task<FlightCrew> AssignCrewMemberAsync(
        int vueloId, int trabajadorId, string rolEnVuelo);
    Task<IReadOnlyCollection<FlightCrew>> GetByFlightAsync(int vueloId);
    Task<bool> ValidateCrewAsync(int vueloId);
    Task RemoveCrewMemberAsync(int flightCrewId);
    Task DeleteAsync(int id);
}