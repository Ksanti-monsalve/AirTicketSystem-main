// src/modules/paymentmethod/Application/Services/PaymentMethodService.cs
using AirTicketSystem.modules.paymentmethod.Application.Interfaces;
using AirTicketSystem.modules.paymentmethod.Application.UseCases;
using AirTicketSystem.modules.paymentmethod.Domain.aggregate;

namespace AirTicketSystem.modules.paymentmethod.Application.Services;

public sealed class PaymentMethodService : IPaymentMethodService
{
    private readonly CreatePaymentMethodUseCase _create;
    private readonly UpdatePaymentMethodUseCase _update;
    private readonly GetPaymentMethodByIdUseCase _getById;

    public PaymentMethodService(
        CreatePaymentMethodUseCase  create,
        UpdatePaymentMethodUseCase  update,
        GetPaymentMethodByIdUseCase getById)
    {
        _create  = create;
        _update  = update;
        _getById = getById;
    }

    public Task<PaymentMethod> CreateAsync(string nombre)
        => _create.ExecuteAsync(nombre);

    public Task<PaymentMethod> UpdateAsync(int metodoPagoId, string nuevoNombre)
        => _update.ExecuteAsync(metodoPagoId, nuevoNombre);

    public Task<PaymentMethod> GetByIdAsync(int metodoPagoId)
        => _getById.ExecuteAsync(metodoPagoId);
}
