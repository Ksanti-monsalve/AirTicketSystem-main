// src/modules/booking/Application/UseCases/ConfirmBookingUseCase.cs
using AirTicketSystem.modules.booking.Domain.aggregate;
using AirTicketSystem.modules.booking.Domain.Repositories;
using AirTicketSystem.modules.bookinghistory.Domain.aggregate;
using AirTicketSystem.modules.bookinghistory.Domain.Repositories;

namespace AirTicketSystem.modules.booking.Application.UseCases;

public sealed class ConfirmBookingUseCase
{
    private readonly IBookingRepository        _bookingRepository;
    private readonly IBookingHistoryRepository _historyRepository;

    public ConfirmBookingUseCase(
        IBookingRepository        bookingRepository,
        IBookingHistoryRepository historyRepository)
    {
        _bookingRepository = bookingRepository;
        _historyRepository = historyRepository;
    }

    public async Task<Booking> ExecuteAsync(
        int id, int? usuarioId = null, CancellationToken cancellationToken = default)
    {
        if (id <= 0)
            throw new ArgumentException("El ID de la reserva no es válido.");

        var booking = await _bookingRepository.FindByIdAsync(id)
            ?? throw new KeyNotFoundException($"No se encontró una reserva con ID {id}.");

        var estadoAnterior = booking.Estado.Valor;
        booking.Confirmar();

        await _bookingRepository.UpdateAsync(booking);
        await _historyRepository.SaveAsync(
            BookingHistory.CrearConfirmacion(booking.Id, usuarioId));

        return booking;
    }
}
