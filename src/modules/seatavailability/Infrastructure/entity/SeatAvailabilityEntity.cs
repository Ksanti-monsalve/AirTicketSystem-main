// src/modules/seatavailability/Infrastructure/entity/SeatAvailabilityEntity.cs
using AirTicketSystem.modules.aircraftseat.Infrastructure.entity;
using AirTicketSystem.modules.booking.Infrastructure.entity;
using AirTicketSystem.modules.bookingpassenger.Infrastructure.entity;
using AirTicketSystem.modules.flight.Infrastructure.entity;
using AirTicketSystem.modules.ticket.Infrastructure.entity;

namespace AirTicketSystem.modules.seatavailability.Infrastructure.entity;

public class SeatAvailabilityEntity
{
    public int Id { get; set; }
    public int VueloId { get; set; }
    public int AsientoId { get; set; }

    // Requisito EXAMEN: número de asiento único por vuelo (ej: 10A)
    public string NumeroAsiento { get; set; } = null!;

    // Requisito EXAMEN: Clase de vuelo asociada al asiento del vuelo
    public int ClaseVueloId { get; set; }
    public string Estado { get; set; } = "DISPONIBLE";
    public int? ReservaId { get; set; }
    public int? TiqueteId { get; set; }

    public FlightEntity Vuelo { get; set; } = null!;
    public AircraftSeatEntity Asiento { get; set; } = null!;
    public BookingEntity? Reserva { get; set; }
    public TicketEntity? Tiquete { get; set; }
    public ICollection<BookingPassengerEntity> PasajerosReserva { get; set; } = new List<BookingPassengerEntity>();
}