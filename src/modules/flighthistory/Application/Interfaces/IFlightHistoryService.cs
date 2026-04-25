// src/modules/flighthistory/Application/Interfaces/IFlightHistoryService.cs
using AirTicketSystem.modules.flighthistory.Domain.aggregate;

namespace AirTicketSystem.modules.flighthistory.Application.Interfaces;

public interface IFlightHistoryService
{
    Task<IReadOnlyCollection<FlightHistory>> GetByFlightAsync(int vueloId);
    Task<FlightHistory?> GetLastChangeAsync(int vueloId);
}
