// src/modules/bookingpassenger/Domain/Repositories/IBookingPassengerRepository.cs
using AirTicketSystem.modules.bookingpassenger.Domain.aggregate;

namespace AirTicketSystem.modules.bookingpassenger.Domain.Repositories;

public interface IBookingPassengerRepository
{
    Task<BookingPassenger?> FindByIdAsync(int id);
    Task<IReadOnlyCollection<BookingPassenger>> FindByReservaAsync(int reservaId);
    Task<BookingPassenger?> FindByReservaAndPersonaAsync(int reservaId, int personaId);
    Task<IReadOnlyCollection<BookingPassenger>> FindByPersonaAsync(int personaId);
    Task<bool> ExistsByReservaAndPersonaAsync(int reservaId, int personaId);
    Task<int> ContarByReservaAsync(int reservaId);
    Task SaveAsync(BookingPassenger passenger);
    Task UpdateAsync(BookingPassenger passenger);
    Task DeleteAsync(int id);
}
