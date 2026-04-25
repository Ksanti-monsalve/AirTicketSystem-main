// src/modules/payment/Application/UseCases/RetryPaymentUseCase.cs
using AirTicketSystem.modules.payment.Domain.aggregate;
using AirTicketSystem.modules.payment.Domain.Repositories;
using AirTicketSystem.modules.paymentmethod.Domain.Repositories;

namespace AirTicketSystem.modules.payment.Application.UseCases;

public sealed class RetryPaymentUseCase
{
    private readonly IPaymentRepository       _paymentRepository;
    private readonly IPaymentMethodRepository _paymentMethodRepository;

    public RetryPaymentUseCase(
        IPaymentRepository       paymentRepository,
        IPaymentMethodRepository paymentMethodRepository)
    {
        _paymentRepository       = paymentRepository;
        _paymentMethodRepository = paymentMethodRepository;
    }

    public async Task<Payment> ExecuteAsync(
        int pagoId,
        int nuevoMetodoPagoId,
        decimal? nuevoMonto = null,
        CancellationToken cancellationToken = default)
    {
        var payment = await _paymentRepository.FindByIdAsync(pagoId)
            ?? throw new KeyNotFoundException(
                $"No se encontró el pago con ID {pagoId}.");

        _ = await _paymentMethodRepository.FindByIdAsync(nuevoMetodoPagoId)
            ?? throw new KeyNotFoundException(
                $"No se encontró el método de pago con ID {nuevoMetodoPagoId}.");

        payment.Reintentar(nuevoMetodoPagoId, nuevoMonto);

        await _paymentRepository.UpdateAsync(payment);
        return payment;
    }
}
