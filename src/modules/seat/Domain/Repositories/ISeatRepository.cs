using AirTicketSystem.modules.seat.Infrastructure.entity;

namespace AirTicketSystem.modules.seat.Domain.Repositories;

public interface ISeatRepository
{
    Task<bool> FlightExistsAsync(int flightId);

    Task<IReadOnlyCollection<SeatDetail>> FindDetailsByFlightAsync(int flightId, string? status = null);
    Task<IReadOnlyCollection<SeatDetail>> FindAvailableDetailsByFlightAndClassAsync(int flightId, int flightClassId);
    Task<IReadOnlyCollection<SeatClassStats>> FindStatsByFlightAsync(int flightId);
    Task<IReadOnlyCollection<SeatDetail>> FindDetailsByBookingAsync(int bookingId);

    Task<SeatEntity?> FindByIdAsync(int seatId);
    Task<SeatDetail?> FindDetailByIdAsync(int seatId);
    Task<int?> FindSeatIdByTicketIdAsync(int ticketId);

    Task SaveAllAsync(IEnumerable<SeatEntity> seats);

    Task<bool> TryReserveAsync(int seatId);
    Task<bool> TryOccupyAsync(int seatId);
    Task<bool> TryReleaseAsync(int seatId);
    Task SetBookingIdAsync(int seatId, int bookingId);
    Task SetTicketIdAsync(int seatId, int ticketId);
}

public sealed record SeatDetail(
    int Id,
    int FlightId,
    string SeatNumber,
    int FlightClassId,
    string FlightClassName,
    string Status,
    int? BookingId,
    int? TicketId);

public sealed record SeatClassStats(
    int FlightClassId,
    string FlightClassName,
    string FlightClassCode,
    int Total,
    int Available,
    int Reserved,
    int Occupied,
    int Blocked);

