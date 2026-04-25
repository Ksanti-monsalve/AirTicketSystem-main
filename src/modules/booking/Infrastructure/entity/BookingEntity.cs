// src/modules/booking/Infrastructure/entity/BookingEntity.cs
using AirTicketSystem.modules.additionalcharge.Infrastructure.entity;
using AirTicketSystem.modules.bookinghistory.Infrastructure.entity;
using AirTicketSystem.modules.bookingpassenger.Infrastructure.entity;
using AirTicketSystem.modules.client.Infrastructure.entity;
using AirTicketSystem.modules.fare.Infrastructure.entity;
using AirTicketSystem.modules.flight.Infrastructure.entity;
using AirTicketSystem.modules.invoice.Infrastructure.entity;
using AirTicketSystem.modules.payment.Infrastructure.entity;

namespace AirTicketSystem.modules.booking.Infrastructure.entity;

public class BookingEntity
{
    public int Id { get; set; }
    public int ClienteId { get; set; }
    public int VueloId { get; set; }
    public int TarifaId { get; set; }
    public string CodigoReserva { get; set; } = null!;
    public DateTime FechaReserva { get; set; } = DateTime.UtcNow;
    public DateTime FechaExpiracion { get; set; }
    public string Estado { get; set; } = "PENDIENTE";
    public decimal ValorTotal { get; set; }
    public string? Observaciones { get; set; }

    public ClientEntity Cliente { get; set; } = null!;
    public FlightEntity Vuelo { get; set; } = null!;
    public FareEntity Tarifa { get; set; } = null!;
    public ICollection<BookingPassengerEntity> Pasajeros { get; set; } = new List<BookingPassengerEntity>();
    public ICollection<BookingHistoryEntity> Historial { get; set; } = new List<BookingHistoryEntity>();
    public ICollection<PaymentEntity> Pagos { get; set; } = new List<PaymentEntity>();
    public ICollection<AdditionalChargeEntity> CargosAdicionales { get; set; } = new List<AdditionalChargeEntity>();
    public InvoiceEntity? Factura { get; set; }
}