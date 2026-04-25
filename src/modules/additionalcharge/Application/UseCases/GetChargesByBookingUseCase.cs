// src/modules/additionalcharge/Application/UseCases/GetChargesByBookingUseCase.cs
using AirTicketSystem.modules.additionalcharge.Domain.aggregate;
using AirTicketSystem.modules.additionalcharge.Domain.Repositories;

namespace AirTicketSystem.modules.additionalcharge.Application.UseCases;

public sealed class GetChargesByBookingUseCase
{
    private readonly IAdditionalChargeRepository _repository;

    public GetChargesByBookingUseCase(IAdditionalChargeRepository repository)
        => _repository = repository;

    public async Task<IReadOnlyCollection<AdditionalCharge>> ExecuteAsync(
        int reservaId, CancellationToken cancellationToken = default)
    {
        if (reservaId <= 0)
            throw new ArgumentException("El ID de la reserva no es válido.");

        return await _repository.FindByReservaAsync(reservaId);
    }
}
