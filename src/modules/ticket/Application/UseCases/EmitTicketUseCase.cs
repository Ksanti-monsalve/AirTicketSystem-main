// src/modules/ticket/Application/UseCases/EmitTicketUseCase.cs
using AirTicketSystem.modules.ticket.Domain.aggregate;
using AirTicketSystem.modules.ticket.Domain.Repositories;
using AirTicketSystem.modules.bookingpassenger.Domain.Repositories;
using AirTicketSystem.modules.booking.Domain.Repositories;
using AirTicketSystem.modules.seatavailability.Domain.Repositories;
using AirTicketSystem.modules.seat.Domain.Repositories;
using AirTicketSystem.shared.context;
using Microsoft.EntityFrameworkCore;

namespace AirTicketSystem.modules.ticket.Application.UseCases;

public sealed class EmitTicketUseCase
{
    private readonly ITicketRepository            _ticketRepository;
    private readonly IBookingPassengerRepository _passengerRepository;
    private readonly IBookingRepository         _bookingRepository;
    private readonly ISeatAvailabilityRepository _seatAvailabilityRepository;
    private readonly ISeatRepository            _seatRepository;
    private readonly AppDbContext                _db;

    public EmitTicketUseCase(
        ITicketRepository           ticketRepository,
        IBookingPassengerRepository passengerRepository,
        IBookingRepository         bookingRepository,
        ISeatAvailabilityRepository seatAvailabilityRepository,
        ISeatRepository            seatRepository,
        AppDbContext                db)
    {
        _ticketRepository            = ticketRepository;
        _passengerRepository         = passengerRepository;
        _bookingRepository           = bookingRepository;
        _seatAvailabilityRepository  = seatAvailabilityRepository;
        _seatRepository              = seatRepository;
        _db                          = db;
    }

    public Task<Ticket> ExecuteAsync(
        int pasajeroReservaId,
        int? asientoConfirmadoId = null,
        CancellationToken cancellationToken = default)
    {
        var strategy = _db.Database.CreateExecutionStrategy();
        return strategy.ExecuteAsync(() => ExecuteInTransactionAsync(
            pasajeroReservaId, asientoConfirmadoId, cancellationToken));
    }

    private async Task<Ticket> ExecuteInTransactionAsync(
        int pasajeroReservaId,
        int? asientoConfirmadoId,
        CancellationToken cancellationToken)
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

        int? asientoFinalAircraftSeatId = asientoConfirmadoId;

        await using var t = await _db.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            if (passenger.AsientoId.HasValue)
            {
                var detalle = await _seatAvailabilityRepository
                    .FindDetalleByDisponibilidadIdAsync(passenger.AsientoId.Value)
                    ?? throw new InvalidOperationException(
                        "El pasajero tiene un asiento asignado, pero no se encontró su disponibilidad.");

                asientoFinalAircraftSeatId = asientoFinalAircraftSeatId ?? detalle.AsientoId;

                if (!await _seatAvailabilityRepository.TryOccupyDisponibilidadAsync(detalle.DisponibilidadId))
                {
                    throw new InvalidOperationException(
                        "No se puede emitir el tiquete porque el asiento no está en estado RESERVADO " +
                        "(puede estar DISPONIBLE, OCUPADO o BLOQUEADO).");
                }
            }

            var ticket = Ticket.Crear(pasajeroReservaId, asientoFinalAircraftSeatId);
            await _ticketRepository.SaveAsync(ticket);

            if (passenger.AsientoId.HasValue)
            {
                await _seatAvailabilityRepository.SetTiqueteIdAsync(
                    passenger.AsientoId.Value, ticket.Id);

                var detalle2 = await _seatAvailabilityRepository
                    .FindDetalleByDisponibilidadIdAsync(passenger.AsientoId.Value);

                if (detalle2 is not null)
                {
                    var all = await _seatRepository.FindDetailsByFlightAsync(detalle2.VueloId);
                    var seat = all.FirstOrDefault(s =>
                        string.Equals(s.SeatNumber, detalle2.NumeroAsiento, StringComparison.OrdinalIgnoreCase));

                    if (seat is not null)
                    {
                        if (!await _seatRepository.TryOccupyAsync(seat.Id))
                        {
                            throw new InvalidOperationException(
                                "No se puede emitir el tiquete porque el asiento no está en estado Reserved (tabla seats).");
                        }

                        await _seatRepository.SetTicketIdAsync(seat.Id, ticket.Id);
                    }
                }
            }

            await t.CommitAsync(cancellationToken);
            return ticket;
        }
        catch
        {
            await t.RollbackAsync(cancellationToken);
            throw;
        }
    }
}
