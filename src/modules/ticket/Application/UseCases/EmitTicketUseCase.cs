// src/modules/ticket/Application/UseCases/EmitTicketUseCase.cs
using AirTicketSystem.modules.ticket.Domain.aggregate;
using AirTicketSystem.modules.ticket.Domain.Repositories;
using AirTicketSystem.modules.bookingpassenger.Domain.Repositories;
using AirTicketSystem.modules.booking.Domain.Repositories;

namespace AirTicketSystem.modules.ticket.Application.UseCases;

public sealed class EmitTicketUseCase
{
    private readonly ITicketRepository           _ticketRepository;
    private readonly IBookingPassengerRepository _passengerRepository;
    private readonly IBookingRepository          _bookingRepository;

    public EmitTicketUseCase(
        ITicketRepository           ticketRepository,
        IBookingPassengerRepository passengerRepository,
        IBookingRepository          bookingRepository)
    {
        _ticketRepository    = ticketRepository;
        _passengerRepository = passengerRepository;
        _bookingRepository   = bookingRepository;
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

        var ticket = Ticket.Crear(pasajeroReservaId, asientoConfirmadoId);
        await _ticketRepository.SaveAsync(ticket);
        return ticket;
    }
}
