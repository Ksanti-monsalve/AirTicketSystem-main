// src/modules/paymentmethod/Application/Interfaces/IPaymentMethodService.cs
using AirTicketSystem.modules.paymentmethod.Domain.aggregate;

namespace AirTicketSystem.modules.paymentmethod.Application.Interfaces;

public interface IPaymentMethodService
{
    Task<PaymentMethod> CreateAsync(string nombre);
    Task<PaymentMethod> UpdateAsync(int metodoPagoId, string nuevoNombre);
    Task<PaymentMethod> GetByIdAsync(int metodoPagoId);
}
