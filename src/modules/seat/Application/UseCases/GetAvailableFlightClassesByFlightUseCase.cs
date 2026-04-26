using AirTicketSystem.modules.seat.Domain.Repositories;

namespace AirTicketSystem.modules.seat.Application.UseCases;

public sealed class GetAvailableFlightClassesByFlightUseCase
{
    private readonly ISeatRepository _repo;

    public GetAvailableFlightClassesByFlightUseCase(ISeatRepository repo) => _repo = repo;

    public async Task<IReadOnlyCollection<SeatClassStats>> ExecuteAsync(
        int flightId,
        CancellationToken ct = default)
    {
        if (flightId <= 0) throw new ArgumentException("Invalid flight id.");
        if (!await _repo.FlightExistsAsync(flightId))
            throw new KeyNotFoundException($"Flight {flightId} not found.");

        var stats = await _repo.FindStatsByFlightAsync(flightId);
        return stats.Where(s => s.Available > 0).ToList().AsReadOnly();
    }
}

