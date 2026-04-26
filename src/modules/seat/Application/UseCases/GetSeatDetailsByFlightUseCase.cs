using AirTicketSystem.modules.seat.Domain.Repositories;

namespace AirTicketSystem.modules.seat.Application.UseCases;

public sealed class GetSeatDetailsByFlightUseCase
{
    private readonly ISeatRepository _repo;

    public GetSeatDetailsByFlightUseCase(ISeatRepository repo) => _repo = repo;

    public async Task<IReadOnlyCollection<SeatDetail>> ExecuteAsync(
        int flightId,
        string? status = null,
        CancellationToken ct = default)
    {
        if (flightId <= 0) throw new ArgumentException("Invalid flight id.");
        if (!await _repo.FlightExistsAsync(flightId))
            throw new KeyNotFoundException($"Flight {flightId} not found.");

        return await _repo.FindDetailsByFlightAsync(flightId, status);
    }
}

