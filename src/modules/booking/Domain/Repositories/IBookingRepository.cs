// src/modules/booking/Domain/Repositories/IBookingRepository.cs
using AirTicketSystem.modules.booking.Domain.aggregate;

namespace AirTicketSystem.modules.booking.Domain.Repositories;

public interface IBookingRepository
{
    Task<Booking?> FindByIdAsync(int id);
    Task<IReadOnlyCollection<Booking>> FindAllAsync();
    Task<Booking?> FindByCodigoReservaAsync(string codigoReserva);
    Task<IReadOnlyCollection<Booking>> FindByClienteAsync(int clienteId);
    Task<IReadOnlyCollection<Booking>> FindByVueloAsync(int vueloId);
    Task<IReadOnlyCollection<Booking>> FindByEstadoAsync(string estado);
    Task<IReadOnlyCollection<Booking>> FindExpirasAsync();
    Task<bool> ExistsByCodigoReservaAsync(string codigoReserva);
    Task<int> ContarByVueloAsync(int vueloId);
    Task SaveAsync(Booking booking);
    Task UpdateAsync(Booking booking);
    Task DeleteAsync(int id);
}
