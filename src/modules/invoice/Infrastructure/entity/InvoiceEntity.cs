// src/modules/invoice/Infrastructure/entity/InvoiceEntity.cs
using AirTicketSystem.modules.booking.Infrastructure.entity;
using AirTicketSystem.modules.person.Infrastructure.entity;

namespace AirTicketSystem.modules.invoice.Infrastructure.entity;

public class InvoiceEntity
{
    public int Id { get; set; }
    public int ReservaId { get; set; }
    public string NumeroFactura { get; set; } = null!;
    public DateTime FechaEmision { get; set; } = DateTime.UtcNow;
    public int DireccionFacturacionId { get; set; }
    public decimal Subtotal { get; set; }
    public decimal Impuestos { get; set; }
    public decimal Total { get; set; }

    public BookingEntity Reserva { get; set; } = null!;
    public PersonAddressEntity DireccionFacturacion { get; set; } = null!;
}