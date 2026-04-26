using AirTicketSystem.modules.seat.Domain.Repositories;

namespace AirTicketSystem.modules.seat.Application.UseCases;

/// <summary>Asientos de la reserva en el mapa <c>seats</c> (número, clase, estado).</summary>
public sealed class GetSeatsByBookingUseCase
{
    private readonly ISeatRepository _repo;

    public GetSeatsByBookingUseCase(ISeatRepository repo) => _repo = repo;

    public async Task<IReadOnlyCollection<SeatDetail>> ExecuteAsync(
        int bookingId,
        CancellationToken cancellationToken = default)
    {
        if (bookingId <= 0) throw new ArgumentException("ID de reserva no válido.");
        return await _repo.FindDetailsByBookingAsync(bookingId);
    }
}
