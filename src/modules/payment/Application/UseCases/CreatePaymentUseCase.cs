// src/modules/payment/Application/UseCases/CreatePaymentUseCase.cs
using AirTicketSystem.modules.payment.Domain.aggregate;
using AirTicketSystem.modules.payment.Domain.Repositories;
using AirTicketSystem.modules.booking.Domain.Repositories;
using AirTicketSystem.modules.paymentmethod.Domain.Repositories;

namespace AirTicketSystem.modules.payment.Application.UseCases;

public sealed class CreatePaymentUseCase
{
    private readonly IPaymentRepository       _paymentRepository;
    private readonly IBookingRepository       _bookingRepository;
    private readonly IPaymentMethodRepository _paymentMethodRepository;

    public CreatePaymentUseCase(
        IPaymentRepository       paymentRepository,
        IBookingRepository       bookingRepository,
        IPaymentMethodRepository paymentMethodRepository)
    {
        _paymentRepository       = paymentRepository;
        _bookingRepository       = bookingRepository;
        _paymentMethodRepository = paymentMethodRepository;
    }

    public async Task<Payment> ExecuteAsync(
        int reservaId,
        int metodoPagoId,
        decimal monto,
        CancellationToken cancellationToken = default)
    {
        var booking = await _bookingRepository.FindByIdAsync(reservaId)
            ?? throw new KeyNotFoundException(
                $"No se encontró la reserva con ID {reservaId}.");

        if (!booking.EstaActiva)
            throw new InvalidOperationException(
                "Solo se puede registrar un pago para una reserva activa (PENDIENTE o CONFIRMADA).");

        _ = await _paymentMethodRepository.FindByIdAsync(metodoPagoId)
            ?? throw new KeyNotFoundException(
                $"No se encontró el método de pago con ID {metodoPagoId}.");

        var payment = Payment.Crear(reservaId, metodoPagoId, monto);

        await _paymentRepository.SaveAsync(payment);
        return payment;
    }
}
