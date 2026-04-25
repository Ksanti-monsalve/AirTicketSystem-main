// src/modules/payment/Application/Services/PaymentService.cs
using AirTicketSystem.modules.payment.Application.Interfaces;
using AirTicketSystem.modules.payment.Application.UseCases;
using AirTicketSystem.modules.payment.Domain.aggregate;

namespace AirTicketSystem.modules.payment.Application.Services;

public sealed class PaymentService : IPaymentService
{
    private readonly CreatePaymentUseCase       _create;
    private readonly ApprovePaymentUseCase      _approve;
    private readonly RejectPaymentUseCase       _reject;
    private readonly RefundPaymentUseCase       _refund;
    private readonly RetryPaymentUseCase        _retry;
    private readonly GetPaymentsByBookingUseCase _getByBooking;

    public PaymentService(
        CreatePaymentUseCase       create,
        ApprovePaymentUseCase      approve,
        RejectPaymentUseCase       reject,
        RefundPaymentUseCase       refund,
        RetryPaymentUseCase        retry,
        GetPaymentsByBookingUseCase getByBooking)
    {
        _create       = create;
        _approve      = approve;
        _reject       = reject;
        _refund       = refund;
        _retry        = retry;
        _getByBooking = getByBooking;
    }

    public Task<Payment> CreateAsync(int reservaId, int metodoPagoId, decimal monto)
        => _create.ExecuteAsync(reservaId, metodoPagoId, monto);

    public Task<Payment> ApproveAsync(int pagoId, string referencia)
        => _approve.ExecuteAsync(pagoId, referencia);

    public Task<Payment> RejectAsync(int pagoId)
        => _reject.ExecuteAsync(pagoId);

    public Task<Payment> RefundAsync(int pagoId)
        => _refund.ExecuteAsync(pagoId);

    public Task<Payment> RetryAsync(int pagoId, int nuevoMetodoPagoId, decimal? nuevoMonto = null)
        => _retry.ExecuteAsync(pagoId, nuevoMetodoPagoId, nuevoMonto);

    public Task<IReadOnlyCollection<Payment>> GetByBookingAsync(int reservaId)
        => _getByBooking.ExecuteAsync(reservaId);
}
