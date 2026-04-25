// src/modules/bookinghistory/Domain/Repositories/IBookingHistoryRepository.cs
using AirTicketSystem.modules.bookinghistory.Domain.aggregate;

namespace AirTicketSystem.modules.bookinghistory.Domain.Repositories;

public interface IBookingHistoryRepository
{
    Task<BookingHistory?> FindByIdAsync(int id);
    Task<IReadOnlyCollection<BookingHistory>> FindByReservaAsync(int reservaId);
    Task<BookingHistory?> FindUltimoCambioByReservaAsync(int reservaId);
    Task SaveAsync(BookingHistory history);
}
