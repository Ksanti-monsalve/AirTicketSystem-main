// src/modules/bookingpassenger/Application/UseCases/AddPassengerUseCase.cs
using AirTicketSystem.modules.bookingpassenger.Domain.aggregate;
using AirTicketSystem.modules.bookingpassenger.Domain.Repositories;
using AirTicketSystem.modules.booking.Domain.Repositories;
using AirTicketSystem.modules.person.Domain.Repositories;

namespace AirTicketSystem.modules.bookingpassenger.Application.UseCases;

public sealed class AddPassengerUseCase
{
    private readonly IBookingPassengerRepository _passengerRepository;
    private readonly IBookingRepository          _bookingRepository;
    private readonly IPersonRepository           _personRepository;

    public AddPassengerUseCase(
        IBookingPassengerRepository passengerRepository,
        IBookingRepository          bookingRepository,
        IPersonRepository           personRepository)
    {
        _passengerRepository = passengerRepository;
        _bookingRepository   = bookingRepository;
        _personRepository    = personRepository;
    }

    public async Task<BookingPassenger> ExecuteAsync(
        int reservaId,
        int personaId,
        string tipoPasajero,
        int? asientoId = null,
        CancellationToken cancellationToken = default)
    {
        var booking = await _bookingRepository.FindByIdAsync(reservaId)
            ?? throw new KeyNotFoundException(
                $"No se encontró una reserva con ID {reservaId}.");

        if (!booking.EstaActiva)
            throw new InvalidOperationException(
                "No se pueden agregar pasajeros a una reserva inactiva.");

        _ = await _personRepository.FindByIdAsync(personaId)
            ?? throw new KeyNotFoundException(
                $"No se encontró una persona con ID {personaId}.");

        if (await _passengerRepository.ExistsByReservaAndPersonaAsync(reservaId, personaId))
            throw new InvalidOperationException(
                "La persona ya está registrada como pasajero en esta reserva.");

        var passenger = BookingPassenger.Crear(reservaId, personaId, tipoPasajero, asientoId);
        await _passengerRepository.SaveAsync(passenger);
        return passenger;
    }
}
