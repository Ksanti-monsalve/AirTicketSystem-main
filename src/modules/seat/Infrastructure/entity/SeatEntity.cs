using AirTicketSystem.modules.booking.Infrastructure.entity;
using AirTicketSystem.modules.flight.Infrastructure.entity;
using AirTicketSystem.modules.flightclass.Infrastructure.entity;
using AirTicketSystem.modules.ticket.Infrastructure.entity;

namespace AirTicketSystem.modules.seat.Infrastructure.entity;

public sealed class SeatEntity
{
    public int Id { get; set; }
    public int FlightId { get; set; }
    public string SeatNumber { get; set; } = null!;
    public int FlightClassId { get; set; }
    public string Status { get; set; } = "Available"; // Available, Reserved, Occupied, Blocked
    public int? BookingId { get; set; }
    public int? TicketId { get; set; }

    public FlightEntity Flight { get; set; } = null!;
    public FlightClassEntity FlightClass { get; set; } = null!;
    public BookingEntity? Booking { get; set; }
    public TicketEntity? Ticket { get; set; }
}

