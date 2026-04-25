// src/modules/ticket/Application/Interfaces/ITicketService.cs
using AirTicketSystem.modules.ticket.Domain.aggregate;

namespace AirTicketSystem.modules.ticket.Application.Interfaces;

public interface ITicketService
{
    Task<Ticket> EmitAsync(int pasajeroReservaId, int? asientoConfirmadoId);
    Task<Ticket> GetByCodeAsync(string codigoTiquete);
    Task<Ticket> CheckInAsync(int id, int? asientoConfirmadoId);
    Task<Ticket> BoardAsync(int id);
    Task<Ticket> VoidAsync(int id);
}
