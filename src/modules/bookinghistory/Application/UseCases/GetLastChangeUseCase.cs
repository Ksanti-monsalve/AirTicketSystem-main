// src/modules/bookinghistory/Application/UseCases/GetLastChangeUseCase.cs
using AirTicketSystem.modules.bookinghistory.Domain.aggregate;
using AirTicketSystem.modules.bookinghistory.Domain.Repositories;

namespace AirTicketSystem.modules.bookinghistory.Application.UseCases;

public sealed class GetLastChangeUseCase
{
    private readonly IBookingHistoryRepository _repository;

    public GetLastChangeUseCase(IBookingHistoryRepository repository) => _repository = repository;

    public async Task<BookingHistory?> ExecuteAsync(
        int reservaId, CancellationToken cancellationToken = default)
    {
        if (reservaId <= 0)
            throw new ArgumentException("El ID de la reserva no es válido.");

        return await _repository.FindUltimoCambioByReservaAsync(reservaId);
    }
}
