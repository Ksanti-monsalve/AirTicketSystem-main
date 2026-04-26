using AirTicketSystem.modules.booking.Domain.Repositories;
using AirTicketSystem.modules.bookingpassenger.Domain.Repositories;
using AirTicketSystem.modules.seatavailability.Domain.Repositories;
using AirTicketSystem.modules.seat.Domain.Repositories;

namespace AirTicketSystem.modules.bookingpassenger.Application.UseCases;

/// <summary>
/// FASE 7/8 (examen): selecciona un asiento DISPONIBLE por vuelo y clase,
/// valida reglas críticas y deja el asiento en estado RESERVADO asociado al pasajero.
/// </summary>
public sealed class SelectSeatForPassengerUseCase
{
    private readonly IBookingPassengerRepository _passengerRepository;
    private readonly IBookingRepository          _bookingRepository;
    private readonly ISeatAvailabilityRepository _seatAvailabilityRepository;
    private readonly ISeatRepository             _seatRepository;

    public SelectSeatForPassengerUseCase(
        IBookingPassengerRepository passengerRepository,
        IBookingRepository          bookingRepository,
        ISeatAvailabilityRepository seatAvailabilityRepository,
        ISeatRepository             seatRepository)
    {
        _passengerRepository        = passengerRepository;
        _bookingRepository          = bookingRepository;
        _seatAvailabilityRepository = seatAvailabilityRepository;
        _seatRepository             = seatRepository;
    }

    public async Task SelectAsync(
        int pasajeroReservaId,
        int vueloId,
        int flightClassId,
        string seatNumber,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(seatNumber))
            throw new ArgumentException("Debe indicar el número de asiento.");
        if (flightClassId <= 0)
            throw new ArgumentException("Debe indicar la clase de vuelo.");

        var passenger = await _passengerRepository.FindByIdAsync(pasajeroReservaId)
            ?? throw new KeyNotFoundException(
                $"No se encontró un pasajero de reserva con ID {pasajeroReservaId}.");

        if (passenger.AsientoId.HasValue)
            throw new InvalidOperationException(
                "Este pasajero ya tiene un asiento asignado. " +
                "No se permite duplicar asiento en otra reserva/pasajero.");

        var booking = await _bookingRepository.FindByIdAsync(passenger.ReservaId)
            ?? throw new KeyNotFoundException(
                $"No se encontró la reserva con ID {passenger.ReservaId}.");

        if (!booking.EstaActiva || booking.EstaExpirada)
            throw new InvalidOperationException(
                $"No se puede seleccionar asiento para una reserva en estado '{booking.Estado}' " +
                "o expirada.");

        // Validación crítica: el asiento debe pertenecer al vuelo de la reserva
        if (booking.VueloId != vueloId)
            throw new InvalidOperationException(
                "El vuelo seleccionado no coincide con el vuelo de la reserva.");

        // EXAMEN literal: validar y reservar sobre tabla 'seats'
        var normalized = seatNumber.Trim().ToUpperInvariant();
        var available = await _seatRepository.FindAvailableDetailsByFlightAndClassAsync(vueloId, flightClassId);
        var seat = available.FirstOrDefault(s =>
            string.Equals(s.SeatNumber, normalized, StringComparison.OrdinalIgnoreCase));

        if (seat is null)
            throw new InvalidOperationException(
                "El asiento no existe, no pertenece a la clase seleccionada, " +
                "o no está disponible para este vuelo (tabla seats).");

        var ok = await _seatRepository.TryReserveAsync(seat.Id);
        if (!ok)
            throw new InvalidOperationException(
                "El asiento ya no está disponible (puede estar Reserved, Occupied o Blocked).");

        await _seatRepository.SetBookingIdAsync(seat.Id, booking.Id);

        // Compatibilidad con el resto del sistema: mantener disponibilidad_asientos + pasajero.asiento_id
        // (pasajeros_reserva.asiento_id referencia disponibilidad_asientos.id en este proyecto).
        var dispDisponibles = await _seatAvailabilityRepository.FindDetallesByVueloAsync(vueloId, estado: "DISPONIBLE");
        var disp = dispDisponibles.FirstOrDefault(d =>
            string.Equals(d.NumeroAsiento, normalized, StringComparison.OrdinalIgnoreCase));
        if (disp is null)
            throw new InvalidOperationException(
                "Se reservó en tabla seats, pero no se encontró la disponibilidad correspondiente (disponibilidad_asientos).");

        var reservado = await _seatAvailabilityRepository.TryReserveDisponibilidadAsync(disp.DisponibilidadId);
        if (!reservado)
            throw new InvalidOperationException(
                "Se reservó en tabla seats, pero la disponibilidad ya no está DISPONIBLE.");

        await _seatAvailabilityRepository.SetReservaIdAsync(disp.DisponibilidadId, booking.Id);

        passenger.AsignarAsiento(disp.DisponibilidadId);
        await _passengerRepository.UpdateAsync(passenger);
    }
}

