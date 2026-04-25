// src/modules/payment/Application/UseCases/GetPaymentsByBookingUseCase.cs
using AirTicketSystem.modules.payment.Domain.aggregate;
using AirTicketSystem.modules.payment.Domain.Repositories;

namespace AirTicketSystem.modules.payment.Application.UseCases;

public sealed class GetPaymentsByBookingUseCase
{
    private readonly IPaymentRepository _repository;

    public GetPaymentsByBookingUseCase(IPaymentRepository repository)
        => _repository = repository;

    public Task<IReadOnlyCollection<Payment>> ExecuteAsync(
        int reservaId,
        CancellationToken cancellationToken = default)
        => _repository.FindByReservaAsync(reservaId);
}
