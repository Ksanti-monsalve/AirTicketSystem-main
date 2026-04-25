// src/modules/payment/Infrastructure/entity/PaymentEntity.cs
using AirTicketSystem.modules.booking.Infrastructure.entity;
using AirTicketSystem.modules.paymentmethod.Infrastructure.entity;

namespace AirTicketSystem.modules.payment.Infrastructure.entity;

public class PaymentEntity
{
    public int Id { get; set; }
    public int ReservaId { get; set; }
    public int MetodoPagoId { get; set; }
    public decimal Monto { get; set; }
    public string Estado { get; set; } = "PENDIENTE";
    public string? ReferenciaPago { get; set; }
    public DateTime? FechaPago { get; set; }
    public DateTime? FechaVencimiento { get; set; }

    public BookingEntity Reserva { get; set; } = null!;
    public PaymentMethodEntity MetodoPago { get; set; } = null!;
}