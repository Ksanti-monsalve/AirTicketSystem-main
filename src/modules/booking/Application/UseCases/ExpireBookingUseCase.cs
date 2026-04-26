// src/modules/booking/Application/UseCases/ExpireBookingUseCase.cs
using AirTicketSystem.modules.booking.Domain.aggregate;
using AirTicketSystem.modules.booking.Domain.Repositories;
using AirTicketSystem.modules.bookinghistory.Domain.aggregate;
using AirTicketSystem.modules.bookinghistory.Domain.Repositories;
using AirTicketSystem.modules.bookingpassenger.Domain.Repositories;
using AirTicketSystem.modules.seatavailability.Domain.Repositories;

namespace AirTicketSystem.modules.booking.Application.UseCases;

public sealed class ExpireBookingUseCase
{
    private readonly IBookingRepository        _bookingRepository;
    private readonly IBookingHistoryRepository _historyRepository;
    private readonly IBookingPassengerRepository _passengerRepository;
    private readonly ISeatAvailabilityRepository _seatAvailabilityRepository;

    public ExpireBookingUseCase(
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
        int id, CancellationToken cancellationToken = default)
    {
        if (id <= 0)
            throw new ArgumentException("El ID de la reserva no es válido.");

        var booking = await _bookingRepository.FindByIdAsync(id)
            ?? throw new KeyNotFoundException($"No se encontró una reserva con ID {id}.");

        booking.Expirar();

        await _bookingRepository.UpdateAsync(booking);

        // EXAMEN: al expirar la reserva, liberar asientos RESERVADOS asociados
        // y limpiar el asiento en pasajeros.
        var pasajeros = await _passengerRepository.FindByReservaAsync(booking.Id);
        foreach (var p in pasajeros)
        {
            if (p.AsientoId is not int dispId) continue;

            _ = await _seatAvailabilityRepository.TryReleaseDisponibilidadAsync(dispId);
            p.LiberarAsiento();
            await _passengerRepository.UpdateAsync(p);
        }

        await _historyRepository.SaveAsync(
            BookingHistory.CrearExpiracion(booking.Id));

        return booking;
    }
}
