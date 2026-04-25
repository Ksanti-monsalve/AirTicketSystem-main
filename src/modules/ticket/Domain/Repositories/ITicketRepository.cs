// src/modules/ticket/Domain/Repositories/ITicketRepository.cs
using AirTicketSystem.modules.ticket.Domain.aggregate;

namespace AirTicketSystem.modules.ticket.Domain.Repositories;

public interface ITicketRepository
{
    Task<Ticket?> FindByIdAsync(int id);
    Task<Ticket?> FindByCodigoTiqueteAsync(string codigoTiquete);
    Task<Ticket?> FindByPasajeroReservaAsync(int pasajeroReservaId);
    Task<IReadOnlyCollection<Ticket>> FindByEstadoAsync(string estado);
    Task<IReadOnlyCollection<Ticket>> FindByVueloAsync(int vueloId);
    Task<bool> ExistsByCodigoTiqueteAsync(string codigoTiquete);
    Task<bool> ExistsByPasajeroReservaAsync(int pasajeroReservaId);
    Task SaveAsync(Ticket ticket);
    Task UpdateAsync(Ticket ticket);
}
