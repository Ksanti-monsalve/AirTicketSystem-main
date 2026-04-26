using AirTicketSystem.modules.seatavailability.Domain.Repositories;

namespace AirTicketSystem.modules.seatavailability.Application.UseCases;

public sealed class GetSeatDetailsByBookingUseCase
{
    private readonly ISeatAvailabilityRepository _repository;

    public GetSeatDetailsByBookingUseCase(ISeatAvailabilityRepository repository)
        => _repository = repository;

    public async Task<IReadOnlyCollection<SeatAvailabilityDetail>> ExecuteAsync(
        int reservaId,
        CancellationToken cancellationToken = default)
    {
        if (reservaId <= 0)
            throw new ArgumentException("El ID de la reserva no es válido.");

        return await _repository.FindDetallesByReservaAsync(reservaId);
    }
}

