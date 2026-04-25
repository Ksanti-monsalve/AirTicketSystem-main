// src/modules/additionalcharge/Application/UseCases/CreateAdditionalChargeUseCase.cs
using AirTicketSystem.modules.additionalcharge.Domain.aggregate;
using AirTicketSystem.modules.additionalcharge.Domain.Repositories;

namespace AirTicketSystem.modules.additionalcharge.Application.UseCases;

public sealed class CreateAdditionalChargeUseCase
{
    private readonly IAdditionalChargeRepository _repository;

    public CreateAdditionalChargeUseCase(IAdditionalChargeRepository repository)
        => _repository = repository;

    public async Task<AdditionalCharge> ExecuteAsync(
        int reservaId,
        string concepto,
        decimal monto,
        CancellationToken cancellationToken = default)
    {
        if (reservaId <= 0)
            throw new ArgumentException("El ID de la reserva no es válido.");

        var charge = AdditionalCharge.Crear(reservaId, concepto, monto);
        await _repository.SaveAsync(charge);
        return charge;
    }
}
