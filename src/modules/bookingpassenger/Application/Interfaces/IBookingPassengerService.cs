// src/modules/bookingpassenger/Application/Interfaces/IBookingPassengerService.cs
using AirTicketSystem.modules.bookingpassenger.Domain.aggregate;

namespace AirTicketSystem.modules.bookingpassenger.Application.Interfaces;

public interface IBookingPassengerService
{
    Task<BookingPassenger> AddAsync(
        int reservaId, int personaId, string tipoPasajero, int? asientoId);
    Task<IReadOnlyCollection<BookingPassenger>> GetByBookingAsync(int reservaId);
    Task<BookingPassenger> AssignSeatAsync(int pasajeroReservaId, int asientoId);
    Task<BookingPassenger> ChangeSeatAsync(int pasajeroReservaId, int nuevoAsientoId);
    Task<BookingPassenger> ReleaseSeatAsync(int pasajeroReservaId);
}
