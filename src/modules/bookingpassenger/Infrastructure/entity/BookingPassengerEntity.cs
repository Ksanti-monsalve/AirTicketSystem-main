// src/modules/bookingpassenger/Infrastructure/entity/BookingPassengerEntity.cs
using AirTicketSystem.modules.booking.Infrastructure.entity;
using AirTicketSystem.modules.checkin.Infrastructure.entity;
using AirTicketSystem.modules.luggage.Infrastructure.entity;
using AirTicketSystem.modules.person.Infrastructure.entity;
using AirTicketSystem.modules.seatavailability.Infrastructure.entity;
using AirTicketSystem.modules.ticket.Infrastructure.entity;

namespace AirTicketSystem.modules.bookingpassenger.Infrastructure.entity;

public class BookingPassengerEntity
{
    public int Id { get; set; }
    public int ReservaId { get; set; }
    public int PersonaId { get; set; }
    public string TipoPasajero { get; set; } = "ADULTO";
    public int? AsientoId { get; set; }

    public BookingEntity Reserva { get; set; } = null!;
    public PersonEntity Persona { get; set; } = null!;
    public SeatAvailabilityEntity? Asiento { get; set; }
    public TicketEntity? Tiquete { get; set; }
    public ICollection<LuggageEntity> Equipajes { get; set; } = new List<LuggageEntity>();
    public CheckInEntity? CheckIn { get; set; }
}