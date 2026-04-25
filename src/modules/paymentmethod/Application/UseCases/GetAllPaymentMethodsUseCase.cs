// src/modules/paymentmethod/Application/UseCases/GetAllPaymentMethodsUseCase.cs
using AirTicketSystem.modules.paymentmethod.Domain.aggregate;
using AirTicketSystem.modules.paymentmethod.Domain.Repositories;

namespace AirTicketSystem.modules.paymentmethod.Application.UseCases;

public sealed class GetAllPaymentMethodsUseCase
{
    private readonly IPaymentMethodRepository _repository;

    public GetAllPaymentMethodsUseCase(IPaymentMethodRepository repository)
        => _repository = repository;

    public async Task<IReadOnlyCollection<PaymentMethod>> ExecuteAsync(
        CancellationToken cancellationToken = default)
        => await _repository.FindAllAsync();
}
