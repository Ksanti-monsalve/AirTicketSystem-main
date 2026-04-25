// src/modules/flight/Application/Interfaces/IFlightService.cs
using AirTicketSystem.modules.flight.Domain.aggregate;

namespace AirTicketSystem.modules.flight.Application.Interfaces;

public interface IFlightService
{
    Task<Flight> CreateAsync(
        int rutaId, int avionId, string numeroVuelo,
        DateTime fechaSalida, DateTime fechaLlegadaEstimada,
        int? puertaEmbarqueId);
    Task<Flight> GetByIdAsync(int id);
    Task<Flight> GetByNumberAndDateAsync(string numeroVuelo, DateTime fecha);
    Task<IReadOnlyCollection<Flight>> GetAllAsync();
    Task<IReadOnlyCollection<Flight>> GetByDateAsync(DateTime fecha);
    Task<IReadOnlyCollection<Flight>> GetByRouteAsync(int rutaId);
    Task<IReadOnlyCollection<Flight>> GetScheduledAsync();
    Task<IReadOnlyCollection<Flight>> SearchAsync(
        int origenId, int destinoId, DateTime fecha);
    Task<IReadOnlyCollection<Flight>> GetWithOpenCheckinAsync();
    Task<Flight> UpdateAsync(
        int id, DateTime fechaSalida,
        DateTime fechaLlegadaEstimada, int? puertaEmbarqueId);
    Task OpenCheckinAsync(int id, DateTime apertura, DateTime cierre);
    Task AssignGateAsync(int id, int puertaEmbarqueId);
    Task StartBoardingAsync(int id);
    Task StartFlightAsync(int id);
    Task RegisterLandingAsync(int id, DateTime fechaLlegadaReal);
    Task CancelAsync(int id, string motivo);
    Task DelayAsync(int id, string motivo);
    Task DivertAsync(int id, string motivo);
    Task DeleteAsync(int id);
}