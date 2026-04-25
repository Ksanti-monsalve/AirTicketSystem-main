// src/modules/seatavailability/Infrastructure/entity/SeatAvailabilityEntity.cs
using AirTicketSystem.modules.aircraftseat.Infrastructure.entity;
using AirTicketSystem.modules.bookingpassenger.Infrastructure.entity;
using AirTicketSystem.modules.flight.Infrastructure.entity;

namespace AirTicketSystem.modules.seatavailability.Infrastructure.entity;

public class SeatAvailabilityEntity
{
    public int Id { get; set; }
    public int VueloId { get; set; }
    public int AsientoId { get; set; }
    public string Estado { get; set; } = "DISPONIBLE";

    public FlightEntity Vuelo { get; set; } = null!;
    public AircraftSeatEntity Asiento { get; set; } = null!;
    public ICollection<BookingPassengerEntity> PasajerosReserva { get; set; } = new List<BookingPassengerEntity>();
}