// src/modules/payment/Application/UseCases/RejectPaymentUseCase.cs
using AirTicketSystem.modules.payment.Domain.aggregate;
using AirTicketSystem.modules.payment.Domain.Repositories;

namespace AirTicketSystem.modules.payment.Application.UseCases;

public sealed class RejectPaymentUseCase
{
    private readonly IPaymentRepository _repository;

    public RejectPaymentUseCase(IPaymentRepository repository)
        => _repository = repository;

    public async Task<Payment> ExecuteAsync(
        int pagoId,
        CancellationToken cancellationToken = default)
    {
        var payment = await _repository.FindByIdAsync(pagoId)
            ?? throw new KeyNotFoundException(
                $"No se encontró el pago con ID {pagoId}.");

        payment.Rechazar();

        await _repository.UpdateAsync(payment);
        return payment;
    }
}
