// src/modules/bookingpassenger/Application/UseCases/AddPassengerUseCase.cs
using AirTicketSystem.modules.bookingpassenger.Domain.aggregate;
using AirTicketSystem.modules.bookingpassenger.Domain.Repositories;
using AirTicketSystem.modules.booking.Domain.Repositories;
using AirTicketSystem.modules.person.Domain.Repositories;
using AirTicketSystem.modules.seatavailability.Domain.Repositories;
using AirTicketSystem.modules.seat.Domain.Repositories;

namespace AirTicketSystem.modules.bookingpassenger.Application.UseCases;

public sealed class AddPassengerUseCase
{
    private readonly IBookingPassengerRepository  _passengerRepository;
    private readonly IBookingRepository           _bookingRepository;
    private readonly IPersonRepository            _personRepository;
    private readonly ISeatAvailabilityRepository  _seatAvailabilityRepository;
    private readonly ISeatRepository              _seatRepository;

    public AddPassengerUseCase(
        IBookingPassengerRepository passengerRepository,
        IBookingRepository          bookingRepository,
        IPersonRepository           personRepository,
        ISeatAvailabilityRepository seatAvailabilityRepository,
        ISeatRepository             seatRepository)
    {
        _passengerRepository        = passengerRepository;
        _bookingRepository          = bookingRepository;
        _personRepository           = personRepository;
        _seatAvailabilityRepository = seatAvailabilityRepository;
        _seatRepository             = seatRepository;
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

        if (asientoId.HasValue)
        {
            // Misma lógica que SelectSeatForPassenger: al elegir asiento al crear reserva, hay que
            // reservar en 'seats' y en disponibilidad_asientos; si no, el pasajero "tiene" asiento
            // pero el mapa 'seets' sigue en Available y luego arroja error al re-seleccionar.
            await ReservarAsientoEnTablasAsync(booking.VueloId, reservaId, asientoId.Value);
        }

        var passenger = BookingPassenger.Crear(reservaId, personaId, tipoPasajero, asientoId);
        await _passengerRepository.SaveAsync(passenger);
        return passenger;
    }

    private async Task ReservarAsientoEnTablasAsync(
        int vueloId,
        int reservaId,
        int disponibilidadId)
    {
        var detalle = await _seatAvailabilityRepository.FindDetalleByDisponibilidadIdAsync(disponibilidadId)
            ?? throw new KeyNotFoundException(
                $"No se encontró disponibilidad con ID {disponibilidadId}.");

        if (detalle.VueloId != vueloId)
            throw new InvalidOperationException(
                "El asiento no pertenece al vuelo de esta reserva.");

        if (detalle.Estado != "DISPONIBLE")
            throw new InvalidOperationException(
                $"El asiento no está libre (estado actual: {detalle.Estado}). " +
                "Elija otro o libere el asiento primero.");

        var normalized = detalle.NumeroAsiento.Trim().ToUpperInvariant();
        var all = await _seatRepository.FindDetailsByFlightAsync(vueloId);
        var seat = all.FirstOrDefault(s =>
            string.Equals(s.SeatNumber, normalized, StringComparison.OrdinalIgnoreCase))
            ?? throw new InvalidOperationException(
                "No se encontró el asiento en el mapa del vuelo (tabla seats). " +
                "Compruebe que el vuelo tuvo generación de asientos.");

        if (!await _seatRepository.TryReserveAsync(seat.Id))
            throw new InvalidOperationException(
                "El asiento no pudo reservarse (puede estar reservado o en uso en el mapa del vuelo).");

        await _seatRepository.SetBookingIdAsync(seat.Id, reservaId);

        if (!await _seatAvailabilityRepository.TryReserveDisponibilidadAsync(disponibilidadId))
        {
            _ = await _seatRepository.TryReleaseAsync(seat.Id);
            throw new InvalidOperationException(
                "Ese puesto dejó de estar libre. Intente con otro asiento.");
        }

        await _seatAvailabilityRepository.SetReservaIdAsync(disponibilidadId, reservaId);
    }
}
