// src/modules/additionalcharge/Application/UseCases/GetTotalChargesByBookingUseCase.cs
using AirTicketSystem.modules.additionalcharge.Domain.Repositories;

namespace AirTicketSystem.modules.additionalcharge.Application.UseCases;

public sealed class GetTotalChargesByBookingUseCase
{
    private readonly IAdditionalChargeRepository _repository;

    public GetTotalChargesByBookingUseCase(IAdditionalChargeRepository repository)
        => _repository = repository;

    public async Task<decimal> ExecuteAsync(int reservaId, CancellationToken cancellationToken = default)
    {
        if (reservaId <= 0)
            throw new ArgumentException("El ID de la reserva no es válido.");

        return await _repository.SumarCargosByReservaAsync(reservaId);
    }
}
