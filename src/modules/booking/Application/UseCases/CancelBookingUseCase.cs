// src/modules/booking/Application/UseCases/CancelBookingUseCase.cs
using AirTicketSystem.modules.booking.Domain.aggregate;
using AirTicketSystem.modules.booking.Domain.Repositories;
using AirTicketSystem.modules.bookinghistory.Domain.aggregate;
using AirTicketSystem.modules.bookinghistory.Domain.Repositories;
using AirTicketSystem.modules.bookingpassenger.Domain.Repositories;
using AirTicketSystem.modules.seatavailability.Domain.Repositories;

namespace AirTicketSystem.modules.booking.Application.UseCases;

public sealed class CancelBookingUseCase
{
    private readonly IBookingRepository        _bookingRepository;
    private readonly IBookingHistoryRepository _historyRepository;
    private readonly IBookingPassengerRepository _passengerRepository;
    private readonly ISeatAvailabilityRepository _seatAvailabilityRepository;

    public CancelBookingUseCase(
        IBookingRepository        bookingRepository,
        IBookingHistoryRepository historyRepository,
        IBookingPassengerRepository passengerRepository,
        ISeatAvailabilityRepository seatAvailabilityRepository)
    {
        _bookingRepository = bookingRepository;
        _historyRepository = historyRepository;
        _passengerRepository = passengerRepository;
        _seatAvailabilityRepository = seatAvailabilityRepository;
    }

    public async Task<Booking> ExecuteAsync(
        int id,
        string motivo,
        int? usuarioId = null,
        CancellationToken cancellationToken = default)
    {
        if (id <= 0)
            throw new ArgumentException("El ID de la reserva no es válido.");

        if (string.IsNullOrWhiteSpace(motivo))
            throw new ArgumentException("El motivo de cancelación es obligatorio.");

        var booking = await _bookingRepository.FindByIdAsync(id)
            ?? throw new KeyNotFoundException($"No se encontró una reserva con ID {id}.");

        var estadoAnterior = booking.Estado.Valor;
        booking.Cancelar();

        await _bookingRepository.UpdateAsync(booking);

        // EXAMEN: al cancelar una reserva, liberar asientos RESERVADOS asociados
        // (restaurar DISPONIBLE y limpiar reserva/tiquete) y limpiar el asiento en pasajeros.
        var pasajeros = await _passengerRepository.FindByReservaAsync(booking.Id);
        foreach (var p in pasajeros)
        {
            if (p.AsientoId is not int dispId) continue;

            _ = await _seatAvailabilityRepository.TryReleaseDisponibilidadAsync(dispId);
            p.LiberarAsiento();
            await _passengerRepository.UpdateAsync(p);
        }

        await _historyRepository.SaveAsync(
            BookingHistory.CrearCancelacion(booking.Id, motivo, usuarioId));

        return booking;
    }
}
