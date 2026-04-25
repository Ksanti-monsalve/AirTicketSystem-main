// src/modules/seatavailability/Application/Interfaces/ISeatAvailabilityService.cs
using AirTicketSystem.modules.seatavailability.Domain.aggregate;

namespace AirTicketSystem.modules.seatavailability.Application.Interfaces;

public interface ISeatAvailabilityService
{
    Task<IReadOnlyCollection<SeatAvailability>> GetAvailableByFlightAsync(int vueloId);
    Task<SeatAvailability> ReserveAsync(int vueloId, int asientoId);
    Task<SeatAvailability> ReleaseAsync(int vueloId, int asientoId);
    Task<SeatAvailability> BlockAsync(int vueloId, int asientoId);
}
