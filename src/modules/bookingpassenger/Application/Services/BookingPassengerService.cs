// src/modules/bookingpassenger/Application/Services/BookingPassengerService.cs
using AirTicketSystem.modules.bookingpassenger.Application.Interfaces;
using AirTicketSystem.modules.bookingpassenger.Application.UseCases;
using AirTicketSystem.modules.bookingpassenger.Domain.aggregate;

namespace AirTicketSystem.modules.bookingpassenger.Application.Services;

public sealed class BookingPassengerService : IBookingPassengerService
{
    private readonly AddPassengerUseCase          _add;
    private readonly GetPassengersByBookingUseCase _getByBooking;
    private readonly AssignSeatUseCase            _assignSeat;
    private readonly ChangeSeatUseCase            _changeSeat;
    private readonly ReleaseSeatUseCase           _releaseSeat;

    public BookingPassengerService(
        AddPassengerUseCase          add,
        GetPassengersByBookingUseCase getByBooking,
        AssignSeatUseCase            assignSeat,
        ChangeSeatUseCase            changeSeat,
        ReleaseSeatUseCase           releaseSeat)
    {
        _add          = add;
        _getByBooking = getByBooking;
        _assignSeat   = assignSeat;
        _changeSeat   = changeSeat;
        _releaseSeat  = releaseSeat;
    }

    public Task<BookingPassenger> AddAsync(
        int reservaId, int personaId, string tipoPasajero, int? asientoId)
        => _add.ExecuteAsync(reservaId, personaId, tipoPasajero, asientoId);

    public Task<IReadOnlyCollection<BookingPassenger>> GetByBookingAsync(int reservaId)
        => _getByBooking.ExecuteAsync(reservaId);

    public Task<BookingPassenger> AssignSeatAsync(int pasajeroReservaId, int asientoId)
        => _assignSeat.ExecuteAsync(pasajeroReservaId, asientoId);

    public Task<BookingPassenger> ChangeSeatAsync(int pasajeroReservaId, int nuevoAsientoId)
        => _changeSeat.ExecuteAsync(pasajeroReservaId, nuevoAsientoId);

    public Task<BookingPassenger> ReleaseSeatAsync(int pasajeroReservaId)
        => _releaseSeat.ExecuteAsync(pasajeroReservaId);
}
