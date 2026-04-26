// src/modules/bookingpassenger/Application/UseCases/ChangeSeatUseCase.cs
using AirTicketSystem.modules.booking.Domain.Repositories;
using AirTicketSystem.modules.bookingpassenger.Domain.aggregate;
using AirTicketSystem.modules.bookingpassenger.Domain.Repositories;
using AirTicketSystem.modules.seat.Domain.Repositories;
using AirTicketSystem.modules.seatavailability.Domain.Repositories;

namespace AirTicketSystem.modules.bookingpassenger.Application.UseCases;

/// <summary>
/// Examen: cambia de asiento liberando <c>disponibilidad_asientos</c> y <c>seats</c> y
/// reutilizando <see cref="SelectSeatForPassengerUseCase"/> para el nuevo asiento.
/// </summary>
public sealed class ChangeSeatUseCase
{
    private readonly IBookingPassengerRepository _passengerRepository;
    private readonly IBookingRepository          _bookingRepository;
    private readonly ISeatAvailabilityRepository  _seatAvailabilityRepository;
    private readonly ISeatRepository              _seatRepository;
    private readonly SelectSeatForPassengerUseCase _selectSeat;

    public ChangeSeatUseCase(
        IBookingPassengerRepository  passengerRepository,
        IBookingRepository          bookingRepository,
        ISeatAvailabilityRepository seatAvailabilityRepository,
        ISeatRepository             seatRepository,
        SelectSeatForPassengerUseCase selectSeat)
    {
        _passengerRepository        = passengerRepository;
        _bookingRepository          = bookingRepository;
        _seatAvailabilityRepository = seatAvailabilityRepository;
        _seatRepository             = seatRepository;
        _selectSeat                 = selectSeat;
    }

    public async Task<BookingPassenger> ExecuteAsync(
        int pasajeroReservaId,
        int flightClassId,
        string nuevoNumeroAsiento,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(nuevoNumeroAsiento))
            throw new ArgumentException("Debe indicar el número de asiento.");
        if (flightClassId <= 0)
            throw new ArgumentException("Debe indicar la clase de vuelo.");

        var passenger = await _passengerRepository.FindByIdAsync(pasajeroReservaId)
            ?? throw new KeyNotFoundException(
                $"No se encontró un pasajero de reserva con ID {pasajeroReservaId}.");

        if (passenger.TipoPasajero.ViajaEnFalda)
            throw new InvalidOperationException("Un infante no puede tener asiento propio.");

        if (!passenger.AsientoId.HasValue)
            throw new InvalidOperationException(
                "El pasajero no tiene asiento asignado. Use «Seleccionar asiento en reserva» primero.");

        var oldDet = await _seatAvailabilityRepository.FindDetalleByDisponibilidadIdAsync(
            passenger.AsientoId.Value)
            ?? throw new InvalidOperationException("No se encontró la disponibilidad del asiento actual.");

        var normalizedNew = nuevoNumeroAsiento.Trim().ToUpperInvariant();
        if (string.Equals(oldDet.NumeroAsiento, normalizedNew, StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException("Indique un asiento distinto al actual.");

        if (oldDet.Estado == "OCUPADO")
            throw new InvalidOperationException(
                "No se puede cambiar de asiento: el actual está ocupado (tiquete emitido).");

        if (oldDet.Estado == "BLOQUEADO")
            throw new InvalidOperationException("No se puede cambiar: el asiento figura bloqueado.");

        if (oldDet.Estado != "RESERVADO")
            throw new InvalidOperationException(
                "Solo se puede cambiar un asiento reservado. " +
                $"Estado actual: {oldDet.Estado}.");

        var booking = await _bookingRepository.FindByIdAsync(passenger.ReservaId)
            ?? throw new KeyNotFoundException("No se encontró la reserva.");

        if (!booking.EstaActiva || booking.EstaExpirada)
            throw new InvalidOperationException(
                "La reserva no admite cambios de asiento en su estado actual.");

        if (booking.VueloId != oldDet.VueloId)
            throw new InvalidOperationException("Inconsistencia entre el vuelo de la reserva y el asiento.");

        if (!await _seatAvailabilityRepository.TryReleaseDisponibilidadAsync(oldDet.DisponibilidadId))
            throw new InvalidOperationException("No se pudo liberar el asiento en disponibilidad.");

        var seatsInFlight = await _seatRepository.FindDetailsByFlightAsync(oldDet.VueloId);
        var oldSeat = seatsInFlight.FirstOrDefault(s =>
            string.Equals(s.SeatNumber, oldDet.NumeroAsiento, StringComparison.OrdinalIgnoreCase));
        if (oldSeat is not null
            && !await _seatRepository.TryReleaseAsync(oldSeat.Id))
        {
            throw new InvalidOperationException(
                "No se pudo liberar el asiento en el registro de asientos del vuelo (tabla seats).");
        }

        passenger.LiberarAsiento();
        await _passengerRepository.UpdateAsync(passenger);

        await _selectSeat.SelectAsync(
            pasajeroReservaId,
            booking.VueloId,
            flightClassId,
            nuevoNumeroAsiento,
            cancellationToken);

        return (await _passengerRepository.FindByIdAsync(pasajeroReservaId))!;
    }
}
