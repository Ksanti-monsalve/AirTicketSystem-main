using AirTicketSystem.modules.seat.Domain.Repositories;

namespace AirTicketSystem.modules.seat.Application.UseCases;

public sealed class GetAvailableSeatsByFlightAndClassUseCase
{
    private readonly ISeatRepository _repo;

    public GetAvailableSeatsByFlightAndClassUseCase(ISeatRepository repo) => _repo = repo;

    public async Task<IReadOnlyCollection<SeatDetail>> ExecuteAsync(
        int flightId,
        int flightClassId,
        CancellationToken ct = default)
    {
        if (flightId <= 0) throw new ArgumentException("Invalid flight id.");
        if (flightClassId <= 0) throw new ArgumentException("Invalid flight class id.");
        if (!await _repo.FlightExistsAsync(flightId))
            throw new KeyNotFoundException($"Flight {flightId} not found.");

        return await _repo.FindAvailableDetailsByFlightAndClassAsync(flightId, flightClassId);
    }
}

