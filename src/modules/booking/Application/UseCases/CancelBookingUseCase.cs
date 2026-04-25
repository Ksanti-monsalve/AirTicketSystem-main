// src/modules/booking/Application/UseCases/CancelBookingUseCase.cs
using AirTicketSystem.modules.booking.Domain.aggregate;
using AirTicketSystem.modules.booking.Domain.Repositories;
using AirTicketSystem.modules.bookinghistory.Domain.aggregate;
using AirTicketSystem.modules.bookinghistory.Domain.Repositories;

namespace AirTicketSystem.modules.booking.Application.UseCases;

public sealed class CancelBookingUseCase
{
    private readonly IBookingRepository        _bookingRepository;
    private readonly IBookingHistoryRepository _historyRepository;

    public CancelBookingUseCase(
        IBookingRepository        bookingRepository,
        IBookingHistoryRepository historyRepository)
    {
        _bookingRepository = bookingRepository;
        _historyRepository = historyRepository;
    }

    public async Task<Booking> ExecuteAsync(
        int id,
        string motivo,
        int? usuarioId = null,
        CancellationToken cancellationToken = default)
    {
        if (id <= 0)
            throw new ArgumentException("El ID de la reserva no es válido.");

        if (string.IsNullOrWhiteSpace(motivo))
            throw new ArgumentException("El motivo de cancelación es obligatorio.");

        var booking = await _bookingRepository.FindByIdAsync(id)
            ?? throw new KeyNotFoundException($"No se encontró una reserva con ID {id}.");

        var estadoAnterior = booking.Estado.Valor;
        booking.Cancelar();

        await _bookingRepository.UpdateAsync(booking);
        await _historyRepository.SaveAsync(
            BookingHistory.CrearCancelacion(booking.Id, motivo, usuarioId));

        return booking;
    }
}
