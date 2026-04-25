// src/modules/booking/Application/Interfaces/IBookingService.cs
using AirTicketSystem.modules.booking.Domain.aggregate;

namespace AirTicketSystem.modules.booking.Application.Interfaces;

public interface IBookingService
{
    Task<Booking> CreateAsync(
        int clienteId, int vueloId, int tarifaId,
        decimal valorTotal, string? observaciones);
    Task<Booking> GetByIdAsync(int id);
    Task<Booking> GetByCodigoAsync(string codigoReserva);
    Task<IReadOnlyCollection<Booking>> GetByClienteAsync(int clienteId);
    Task<Booking> ConfirmAsync(int id, int? usuarioId);
    Task<Booking> CancelAsync(int id, string motivo, int? usuarioId);
    Task<Booking> ExpireAsync(int id);
    Task<Booking> ExtendAsync(int id, int horas);
    Task<Booking> UpdateObservationsAsync(int id, string? observaciones);
    Task DeleteAsync(int id);
}
