// src/modules/ticket/Infrastructure/entity/TicketEntity.cs
using AirTicketSystem.modules.aircraftseat.Infrastructure.entity;
using AirTicketSystem.modules.bookingpassenger.Infrastructure.entity;

namespace AirTicketSystem.modules.ticket.Infrastructure.entity;

public class TicketEntity
{
    public int Id { get; set; }
    public int PasajeroReservaId { get; set; }
    public string CodigoTiquete { get; set; } = null!;
    public int? AsientoConfirmadoId { get; set; }
    public DateTime FechaEmision { get; set; } = DateTime.UtcNow;
    public string Estado { get; set; } = "EMITIDO";

    public BookingPassengerEntity PasajeroReserva { get; set; } = null!;
    public AircraftSeatEntity? AsientoConfirmado { get; set; }
}