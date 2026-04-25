// src/modules/payment/Application/Interfaces/IPaymentService.cs
using AirTicketSystem.modules.payment.Domain.aggregate;

namespace AirTicketSystem.modules.payment.Application.Interfaces;

public interface IPaymentService
{
    Task<Payment> CreateAsync(int reservaId, int metodoPagoId, decimal monto);
    Task<Payment> ApproveAsync(int pagoId, string referencia);
    Task<Payment> RejectAsync(int pagoId);
    Task<Payment> RefundAsync(int pagoId);
    Task<Payment> RetryAsync(int pagoId, int nuevoMetodoPagoId, decimal? nuevoMonto = null);
    Task<IReadOnlyCollection<Payment>> GetByBookingAsync(int reservaId);
}
