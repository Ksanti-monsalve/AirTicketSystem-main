// src/modules/checkin/Infrastructure/entity/CheckInEntity.cs
using AirTicketSystem.modules.boardingpass.Infrastructure.entity;
using AirTicketSystem.modules.bookingpassenger.Infrastructure.entity;
using AirTicketSystem.modules.worker.Infrastructure.entity;

namespace AirTicketSystem.modules.checkin.Infrastructure.entity;

public class CheckInEntity
{
    public int Id { get; set; }
    public int PasajeroReservaId { get; set; }
    public string Tipo { get; set; } = null!;
    public DateTime FechaCheckin { get; set; } = DateTime.UtcNow;
    public int? TrabajadorId { get; set; }
    public string Estado { get; set; } = "PENDIENTE";

    public BookingPassengerEntity PasajeroReserva { get; set; } = null!;
    public WorkerEntity? Trabajador { get; set; }
    public BoardingPassEntity? PaseAbordar { get; set; }
}