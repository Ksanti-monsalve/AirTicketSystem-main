// src/modules/paymentmethod/Infrastructure/entity/PaymentMethodEntity.cs
using AirTicketSystem.modules.payment.Infrastructure.entity;

namespace AirTicketSystem.modules.paymentmethod.Infrastructure.entity;

public class PaymentMethodEntity
{
    public int Id { get; set; }
    public string Nombre { get; set; } = null!;
    public ICollection<PaymentEntity> Pagos { get; set; } = new List<PaymentEntity>();
}