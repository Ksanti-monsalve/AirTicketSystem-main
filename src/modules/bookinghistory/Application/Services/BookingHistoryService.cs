// src/modules/bookinghistory/Application/Services/BookingHistoryService.cs
using AirTicketSystem.modules.bookinghistory.Application.Interfaces;
using AirTicketSystem.modules.bookinghistory.Application.UseCases;
using AirTicketSystem.modules.bookinghistory.Domain.aggregate;

namespace AirTicketSystem.modules.bookinghistory.Application.Services;

public sealed class BookingHistoryService : IBookingHistoryService
{
    private readonly GetHistoryByBookingUseCase _getByBooking;
    private readonly GetLastChangeUseCase       _getLastChange;

    public BookingHistoryService(
        GetHistoryByBookingUseCase getByBooking,
        GetLastChangeUseCase       getLastChange)
    {
        _getByBooking  = getByBooking;
        _getLastChange = getLastChange;
    }

    public Task<IReadOnlyCollection<BookingHistory>> GetByBookingAsync(int reservaId)
        => _getByBooking.ExecuteAsync(reservaId);

    public Task<BookingHistory?> GetLastChangeAsync(int reservaId)
        => _getLastChange.ExecuteAsync(reservaId);
}
