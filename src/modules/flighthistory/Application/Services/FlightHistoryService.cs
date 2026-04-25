// src/modules/flighthistory/Application/Services/FlightHistoryService.cs
using AirTicketSystem.modules.flighthistory.Application.Interfaces;
using AirTicketSystem.modules.flighthistory.Application.UseCases;
using AirTicketSystem.modules.flighthistory.Domain.aggregate;

namespace AirTicketSystem.modules.flighthistory.Application.Services;

public sealed class FlightHistoryService : IFlightHistoryService
{
    private readonly GetFlightHistoryUseCase  _getByFlight;
    private readonly GetLastFlightChangeUseCase _getLastChange;

    public FlightHistoryService(
        GetFlightHistoryUseCase     getByFlight,
        GetLastFlightChangeUseCase  getLastChange)
    {
        _getByFlight   = getByFlight;
        _getLastChange = getLastChange;
    }

    public Task<IReadOnlyCollection<FlightHistory>> GetByFlightAsync(int vueloId)
        => _getByFlight.ExecuteAsync(vueloId);

    public Task<FlightHistory?> GetLastChangeAsync(int vueloId)
        => _getLastChange.ExecuteAsync(vueloId);
}
