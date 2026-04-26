// src/modules/ticket/Application/UseCases/EmitTicketUseCase.cs
using AirTicketSystem.modules.ticket.Domain.aggregate;
using AirTicketSystem.modules.ticket.Domain.Repositories;
using AirTicketSystem.modules.bookingpassenger.Domain.Repositories;
using AirTicketSystem.modules.booking.Domain.Repositories;
using AirTicketSystem.modules.seatavailability.Domain.Repositories;
using AirTicketSystem.modules.seat.Domain.Repositories;

namespace AirTicketSystem.modules.ticket.Application.UseCases;

public sealed class EmitTicketUseCase
{
    private readonly ITicketRepository           _ticketRepository;
    private readonly IBookingPassengerRepository _passengerRepository;
    private readonly IBookingRepository          _bookingRepository;
    private readonly ISeatAvailabilityRepository _seatAvailabilityRepository;
    private readonly ISeatRepository             _seatRepository;

    public EmitTicketUseCase(
        ITicketRepository           ticketRepository,
        IBookingPassengerRepository passengerRepository,
        IBookingRepository          bookingRepository,
        ISeatAvailabilityRepository seatAvailabilityRepository,
        ISeatRepository             seatRepository)
    {
        _ticketRepository    = ticketRepository;
        _passengerRepository = passengerRepository;
        _bookingRepository   = bookingRepository;
        _seatAvailabilityRepository = seatAvailabilityRepository;
        _seatRepository      = seatRepository;
    }

    public async Task<Ticket> ExecuteAsync(
        int pasajeroReservaId,
        int? asientoConfirmadoId = null,
        CancellationToken cancellationToken = default)
    {
        var passenger = await _passengerRepository.FindByIdAsync(pasajeroReservaId)
            ?? throw new KeyNotFoundException(
                $"No se encontró un pasajero de reserva con ID {pasajeroReservaId}.");

        var booking = await _bookingRepository.FindByIdAsync(passenger.ReservaId)
            ?? throw new KeyNotFoundException(
                $"No se encontró la reserva con ID {passenger.ReservaId}.");

        if (!booking.PuedeEmitirTiquetes)
            throw new InvalidOperationException(
                $"La reserva no está en estado válido para emitir tiquetes. " +
                $"Estado: '{booking.Estado}'.");

        if (await _ticketRepository.ExistsByPasajeroReservaAsync(pasajeroReservaId))
            throw new InvalidOperationException(
                "Ya existe un tiquete emitido para este pasajero en esta reserva.");

        // Integración con asientos (examen): si el pasajero tiene asiento reservado por vuelo,
        // se confirma el asiento en el tiquete y se cambia el estado RESERVADO -> OCUPADO.
        int? asientoFinalAircraftSeatId = asientoConfirmadoId;

        if (passenger.AsientoId.HasValue)
        {
            var detalle = await _seatAvailabilityRepository
                .FindDetalleByDisponibilidadIdAsync(passenger.AsientoId.Value)
                ?? throw new InvalidOperationException(
                    "El pasajero tiene un asiento asignado, pero no se encontró su disponibilidad.");

            // El tiquete guarda el asiento físico del avión (AircraftSeat.Id)
            asientoFinalAircraftSeatId = detalle.AsientoId;

            // Solo permitir emitir si el asiento está RESERVADO; al emitir pasa a OCUPADO.
            var ok = await _seatAvailabilityRepository
                .TryOccupyDisponibilidadAsync(detalle.DisponibilidadId);

            if (!ok)
                throw new InvalidOperationException(
                    "No se puede emitir el tiquete porque el asiento no está en estado RESERVADO " +
                    "(puede estar DISPONIBLE, OCUPADO o BLOQUEADO).");
        }

        var ticket = Ticket.Crear(pasajeroReservaId, asientoFinalAircraftSeatId);
        await _ticketRepository.SaveAsync(ticket);

        // Persistencia (examen): asociar disponibilidad-asiento al tiquete emitido
        if (passenger.AsientoId.HasValue)
            await _seatAvailabilityRepository.SetTiqueteIdAsync(passenger.AsientoId.Value, ticket.Id);

        // EXAMEN (literal): asociar Seat.TicketId y marcar Occupied en tabla 'seats'
        if (passenger.AsientoId.HasValue)
        {
            // Ubicar Seat por (flightId + seatNumber) para mantener consistencia con el modelo Seat
            var detalle = await _seatAvailabilityRepository
                .FindDetalleByDisponibilidadIdAsync(passenger.AsientoId.Value);

            if (detalle is not null)
            {
                // Buscar el seat id que corresponda a ese vuelo y número
                var all = await _seatRepository.FindDetailsByFlightAsync(detalle.VueloId);
                var seat = all.FirstOrDefault(s =>
                    string.Equals(s.SeatNumber, detalle.NumeroAsiento, StringComparison.OrdinalIgnoreCase));

                if (seat is not null)
                {
                    // En seats debe estar Reserved para poder pasar a Occupied
                    var ok2 = await _seatRepository.TryOccupyAsync(seat.Id);
                    if (!ok2)
                        throw new InvalidOperationException(
                            "No se puede emitir el tiquete porque el asiento no está en estado Reserved (tabla seats).");
                    await _seatRepository.SetTicketIdAsync(seat.Id, ticket.Id);
                }
            }
        }

        return ticket;
    }
}
