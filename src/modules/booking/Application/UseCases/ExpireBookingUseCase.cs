// src/modules/booking/Application/UseCases/ExpireBookingUseCase.cs
using AirTicketSystem.modules.booking.Domain.aggregate;
using AirTicketSystem.modules.booking.Domain.Repositories;
using AirTicketSystem.modules.bookinghistory.Domain.aggregate;
using AirTicketSystem.modules.bookinghistory.Domain.Repositories;

namespace AirTicketSystem.modules.booking.Application.UseCases;

public sealed class ExpireBookingUseCase
{
    private readonly IBookingRepository        _bookingRepository;
    private readonly IBookingHistoryRepository _historyRepository;

    public ExpireBookingUseCase(
        IBookingRepository        bookingRepository,
        IBookingHistoryRepository historyRepository)
    {
        _bookingRepository = bookingRepository;
        _historyRepository = historyRepository;
    }

    public async Task<Booking> ExecuteAsync(
        int id, CancellationToken cancellationToken = default)
    {
        if (id <= 0)
            throw new ArgumentException("El ID de la reserva no es válido.");

        var booking = await _bookingRepository.FindByIdAsync(id)
            ?? throw new KeyNotFoundException($"No se encontró una reserva con ID {id}.");

        booking.Expirar();

        await _bookingRepository.UpdateAsync(booking);
        await _historyRepository.SaveAsync(
            BookingHistory.CrearExpiracion(booking.Id));

        return booking;
    }
}
