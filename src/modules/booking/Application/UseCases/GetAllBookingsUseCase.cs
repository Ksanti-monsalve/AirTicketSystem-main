using AirTicketSystem.modules.booking.Domain.aggregate;
using AirTicketSystem.modules.booking.Domain.Repositories;

namespace AirTicketSystem.modules.booking.Application.UseCases;

public sealed class GetAllBookingsUseCase
{
    private readonly IBookingRepository _repo;

    public GetAllBookingsUseCase(IBookingRepository repo) => _repo = repo;

    public Task<IReadOnlyCollection<Booking>> ExecuteAsync(
        CancellationToken cancellationToken = default)
        => _repo.FindAllAsync();
}
