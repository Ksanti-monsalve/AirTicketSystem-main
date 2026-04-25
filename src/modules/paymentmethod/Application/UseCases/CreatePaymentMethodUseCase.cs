// src/modules/paymentmethod/Application/UseCases/CreatePaymentMethodUseCase.cs
using AirTicketSystem.modules.paymentmethod.Domain.aggregate;
using AirTicketSystem.modules.paymentmethod.Domain.Repositories;

namespace AirTicketSystem.modules.paymentmethod.Application.UseCases;

public sealed class CreatePaymentMethodUseCase
{
    private readonly IPaymentMethodRepository _repository;

    public CreatePaymentMethodUseCase(IPaymentMethodRepository repository)
        => _repository = repository;

    public async Task<PaymentMethod> ExecuteAsync(
        string nombre,
        CancellationToken cancellationToken = default)
    {
        if (await _repository.ExistsByNombreAsync(nombre))
            throw new InvalidOperationException(
                $"Ya existe un método de pago con el nombre '{nombre}'.");

        var method = PaymentMethod.Crear(nombre);

        await _repository.SaveAsync(method);
        return method;
    }
}
