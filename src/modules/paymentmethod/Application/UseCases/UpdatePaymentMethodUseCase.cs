// src/modules/paymentmethod/Application/UseCases/UpdatePaymentMethodUseCase.cs
using AirTicketSystem.modules.paymentmethod.Domain.aggregate;
using AirTicketSystem.modules.paymentmethod.Domain.Repositories;

namespace AirTicketSystem.modules.paymentmethod.Application.UseCases;

public sealed class UpdatePaymentMethodUseCase
{
    private readonly IPaymentMethodRepository _repository;

    public UpdatePaymentMethodUseCase(IPaymentMethodRepository repository)
        => _repository = repository;

    public async Task<PaymentMethod> ExecuteAsync(
        int metodoPagoId,
        string nuevoNombre,
        CancellationToken cancellationToken = default)
    {
        var method = await _repository.FindByIdAsync(metodoPagoId)
            ?? throw new KeyNotFoundException(
                $"No se encontró el método de pago con ID {metodoPagoId}.");

        if (await _repository.ExistsByNombreAsync(nuevoNombre))
            throw new InvalidOperationException(
                $"Ya existe un método de pago con el nombre '{nuevoNombre}'.");

        method.ActualizarNombre(nuevoNombre);

        await _repository.UpdateAsync(method);
        return method;
    }
}
