// src/modules/payment/Application/UseCases/ApprovePaymentUseCase.cs
using AirTicketSystem.modules.payment.Domain.aggregate;
using AirTicketSystem.modules.payment.Domain.Repositories;

namespace AirTicketSystem.modules.payment.Application.UseCases;

public sealed class ApprovePaymentUseCase
{
    private readonly IPaymentRepository _repository;

    public ApprovePaymentUseCase(IPaymentRepository repository)
        => _repository = repository;

    public async Task<Payment> ExecuteAsync(
        int pagoId,
        string referencia,
        CancellationToken cancellationToken = default)
    {
        var payment = await _repository.FindByIdAsync(pagoId)
            ?? throw new KeyNotFoundException(
                $"No se encontró el pago con ID {pagoId}.");

        payment.Aprobar(referencia);

        await _repository.UpdateAsync(payment);
        return payment;
    }
}
