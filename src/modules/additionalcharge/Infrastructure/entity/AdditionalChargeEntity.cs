// src/modules/additionalcharge/Infrastructure/entity/AdditionalChargeEntity.cs
using AirTicketSystem.modules.booking.Infrastructure.entity;

namespace AirTicketSystem.modules.additionalcharge.Infrastructure.entity;

public class AdditionalChargeEntity
{
    public int Id { get; set; }
    public int ReservaId { get; set; }
    public string Concepto { get; set; } = null!;
    public decimal Monto { get; set; }
    public DateTime Fecha { get; set; } = DateTime.UtcNow;

    public BookingEntity Reserva { get; set; } = null!;
}