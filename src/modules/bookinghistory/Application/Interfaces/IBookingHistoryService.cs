// src/modules/bookinghistory/Application/Interfaces/IBookingHistoryService.cs
using AirTicketSystem.modules.bookinghistory.Domain.aggregate;

namespace AirTicketSystem.modules.bookinghistory.Application.Interfaces;

public interface IBookingHistoryService
{
    Task<IReadOnlyCollection<BookingHistory>> GetByBookingAsync(int reservaId);
    Task<BookingHistory?> GetLastChangeAsync(int reservaId);
}
