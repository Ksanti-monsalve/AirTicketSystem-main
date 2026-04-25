// src/modules/bookinghistory/Application/UseCases/GetHistoryByBookingUseCase.cs
using AirTicketSystem.modules.bookinghistory.Domain.aggregate;
using AirTicketSystem.modules.bookinghistory.Domain.Repositories;

namespace AirTicketSystem.modules.bookinghistory.Application.UseCases;

public sealed class GetHistoryByBookingUseCase
{
    private readonly IBookingHistoryRepository _repository;

    public GetHistoryByBookingUseCase(IBookingHistoryRepository repository) => _repository = repository;

    public async Task<IReadOnlyCollection<BookingHistory>> ExecuteAsync(
        int reservaId, CancellationToken cancellationToken = default)
    {
        if (reservaId <= 0)
            throw new ArgumentException("El ID de la reserva no es válido.");

        return await _repository.FindByReservaAsync(reservaId);
    }
}
