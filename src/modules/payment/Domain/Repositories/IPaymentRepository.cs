// src/modules/payment/Domain/Repositories/IPaymentRepository.cs
using AirTicketSystem.modules.payment.Domain.aggregate;

namespace AirTicketSystem.modules.payment.Domain.Repositories;

public interface IPaymentRepository
{
    Task<Payment?> FindByIdAsync(int id);
    Task<IReadOnlyCollection<Payment>> FindByReservaAsync(int reservaId);
    Task<Payment?> FindAprobadoByReservaAsync(int reservaId);
    Task<IReadOnlyCollection<Payment>> FindByEstadoAsync(string estado);
    Task<IReadOnlyCollection<Payment>> FindVencidosAsync();
    Task<bool> ReservaTienePagoAprobadoAsync(int reservaId);
    Task<decimal> SumarPagosAprobadosByReservaAsync(int reservaId);
    Task SaveAsync(Payment payment);
    Task UpdateAsync(Payment payment);
}
