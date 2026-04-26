using Microsoft.EntityFrameworkCore;
using AirTicketSystem.shared.context;
using AirTicketSystem.modules.seat.Domain.Repositories;
using AirTicketSystem.modules.seat.Infrastructure.entity;

namespace AirTicketSystem.modules.seat.Infrastructure.repository;

public sealed class SeatRepository : ISeatRepository
{
    private readonly AppDbContext _context;

    public SeatRepository(AppDbContext context) => _context = context;

    public async Task<bool> FlightExistsAsync(int flightId)
        => await _context.Vuelos.AsNoTracking().AnyAsync(f => f.Id == flightId);

    public async Task<IReadOnlyCollection<SeatDetail>> FindDetailsByFlightAsync(int flightId, string? status = null)
    {
        var query = _context.Seats
            .AsNoTracking()
            .Include(s => s.FlightClass)
            .Where(s => s.FlightId == flightId);

        if (!string.IsNullOrWhiteSpace(status))
            query = query.Where(s => s.Status == status);

        var items = await query
            .OrderBy(s => s.SeatNumber)
            .Select(s => new SeatDetail(
                s.Id,
                s.FlightId,
                s.SeatNumber,
                s.FlightClassId,
                s.FlightClass.Name,
                s.Status,
                s.BookingId,
                s.TicketId))
            .ToListAsync();

        return items.AsReadOnly();
    }

    public async Task<IReadOnlyCollection<SeatDetail>> FindAvailableDetailsByFlightAndClassAsync(int flightId, int flightClassId)
    {
        var items = await _context.Seats
            .AsNoTracking()
            .Include(s => s.FlightClass)
            .Where(s =>
                s.FlightId == flightId &&
                s.FlightClassId == flightClassId &&
                s.Status == "Available")
            .OrderBy(s => s.SeatNumber)
            .Select(s => new SeatDetail(
                s.Id,
                s.FlightId,
                s.SeatNumber,
                s.FlightClassId,
                s.FlightClass.Name,
                s.Status,
                s.BookingId,
                s.TicketId))
            .ToListAsync();

        return items.AsReadOnly();
    }

    public async Task<IReadOnlyCollection<SeatClassStats>> FindStatsByFlightAsync(int flightId)
    {
        // Importante: evitar OrderBy sobre un tipo proyectado (record) porque EF puede no traducirlo.
        // Proyectamos a anónimo (SQL traducible), ordenamos y luego mapeamos a SeatClassStats en memoria.
        var rows = await _context.Seats
            .AsNoTracking()
            .Where(s => s.FlightId == flightId)
            .GroupBy(s => new { s.FlightClassId, Name = s.FlightClass.Name, Code = s.FlightClass.Code })
            .Select(g => new
            {
                g.Key.FlightClassId,
                g.Key.Name,
                g.Key.Code,
                Total     = g.Count(),
                Available = g.Count(x => x.Status == "Available"),
                Reserved  = g.Count(x => x.Status == "Reserved"),
                Occupied  = g.Count(x => x.Status == "Occupied"),
                Blocked   = g.Count(x => x.Status == "Blocked")
            })
            .OrderBy(x => x.Name)
            .ToListAsync();

        return rows
            .Select(x => new SeatClassStats(
                x.FlightClassId,
                x.Name,
                x.Code,
                x.Total,
                x.Available,
                x.Reserved,
                x.Occupied,
                x.Blocked))
            .ToList()
            .AsReadOnly();
    }

    public async Task<IReadOnlyCollection<SeatDetail>> FindDetailsByBookingAsync(int bookingId)
    {
        var items = await _context.Seats
            .AsNoTracking()
            .Include(s => s.FlightClass)
            .Where(s => s.BookingId == bookingId)
            .OrderBy(s => s.SeatNumber)
            .Select(s => new SeatDetail(
                s.Id,
                s.FlightId,
                s.SeatNumber,
                s.FlightClassId,
                s.FlightClass.Name,
                s.Status,
                s.BookingId,
                s.TicketId))
            .ToListAsync();

        return items.AsReadOnly();
    }

    public async Task<SeatEntity?> FindByIdAsync(int seatId)
        => await _context.Seats
            .Include(s => s.FlightClass)
            .FirstOrDefaultAsync(s => s.Id == seatId);

    public async Task<SeatDetail?> FindDetailByIdAsync(int seatId)
        => await _context.Seats
            .AsNoTracking()
            .Include(s => s.FlightClass)
            .Where(s => s.Id == seatId)
            .Select(s => new SeatDetail(
                s.Id,
                s.FlightId,
                s.SeatNumber,
                s.FlightClassId,
                s.FlightClass.Name,
                s.Status,
                s.BookingId,
                s.TicketId))
            .FirstOrDefaultAsync();

    public async Task<int?> FindSeatIdByTicketIdAsync(int ticketId)
        => await _context.Seats
            .AsNoTracking()
            .Where(s => s.TicketId == ticketId)
            .Select(s => (int?)s.Id)
            .FirstOrDefaultAsync();

    public async Task SaveAllAsync(IEnumerable<SeatEntity> seats)
    {
        await _context.Seats.AddRangeAsync(seats);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> TryReserveAsync(int seatId)
    {
        var affected = await _context.Seats
            .Where(s => s.Id == seatId && s.Status == "Available")
            .ExecuteUpdateAsync(setters => setters.SetProperty(s => s.Status, "Reserved"));

        return affected == 1;
    }

    public async Task<bool> TryOccupyAsync(int seatId)
    {
        var affected = await _context.Seats
            .Where(s => s.Id == seatId && s.Status == "Reserved")
            .ExecuteUpdateAsync(setters => setters.SetProperty(s => s.Status, "Occupied"));

        return affected == 1;
    }

    public async Task<bool> TryReleaseAsync(int seatId)
    {
        var affected = await _context.Seats
            .Where(s => s.Id == seatId && s.Status == "Reserved")
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(s => s.Status, "Available")
                .SetProperty(s => s.BookingId, (int?)null)
                .SetProperty(s => s.TicketId, (int?)null));

        return affected == 1;
    }

    public async Task SetBookingIdAsync(int seatId, int bookingId)
    {
        if (bookingId <= 0) throw new ArgumentException("Invalid booking id.");
        _ = await _context.Seats
            .Where(s => s.Id == seatId)
            .ExecuteUpdateAsync(setters => setters.SetProperty(s => s.BookingId, bookingId));
    }

    public async Task SetTicketIdAsync(int seatId, int ticketId)
    {
        if (ticketId <= 0) throw new ArgumentException("Invalid ticket id.");
        _ = await _context.Seats
            .Where(s => s.Id == seatId)
            .ExecuteUpdateAsync(setters => setters.SetProperty(s => s.TicketId, ticketId));
    }
}

