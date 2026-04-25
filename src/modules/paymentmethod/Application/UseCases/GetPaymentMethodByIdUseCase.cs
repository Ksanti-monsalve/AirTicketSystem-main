// src/modules/paymentmethod/Application/UseCases/GetPaymentMethodByIdUseCase.cs
using AirTicketSystem.modules.paymentmethod.Domain.aggregate;
using AirTicketSystem.modules.paymentmethod.Domain.Repositories;

namespace AirTicketSystem.modules.paymentmethod.Application.UseCases;

public sealed class GetPaymentMethodByIdUseCase
{
    private readonly IPaymentMethodRepository _repository;

    public GetPaymentMethodByIdUseCase(IPaymentMethodRepository repository)
        => _repository = repository;

    public async Task<PaymentMethod> ExecuteAsync(
        int metodoPagoId,
        CancellationToken cancellationToken = default)
        => await _repository.FindByIdAsync(metodoPagoId)
            ?? throw new KeyNotFoundException(
                $"No se encontró el método de pago con ID {metodoPagoId}.");
}
