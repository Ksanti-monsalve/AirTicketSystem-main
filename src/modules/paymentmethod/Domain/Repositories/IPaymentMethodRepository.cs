// src/modules/paymentmethod/Domain/Repositories/IPaymentMethodRepository.cs
using AirTicketSystem.modules.paymentmethod.Domain.aggregate;

namespace AirTicketSystem.modules.paymentmethod.Domain.Repositories;

public interface IPaymentMethodRepository
{
    Task<PaymentMethod?> FindByIdAsync(int id);
    Task<IReadOnlyCollection<PaymentMethod>> FindAllAsync();
    Task<PaymentMethod?> FindByNombreAsync(string nombre);
    Task<bool> ExistsByNombreAsync(string nombre);
    Task SaveAsync(PaymentMethod method);
    Task UpdateAsync(PaymentMethod method);
}
