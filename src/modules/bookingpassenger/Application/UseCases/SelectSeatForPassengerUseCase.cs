using AirTicketSystem.modules.booking.Domain.Repositories;
using AirTicketSystem.modules.bookingpassenger.Domain.Repositories;
using AirTicketSystem.modules.seatavailability.Domain.Repositories;
using AirTicketSystem.modules.seat.Domain.Repositories;
using AirTicketSystem.shared.context;
using Microsoft.EntityFrameworkCore;

namespace AirTicketSystem.modules.bookingpassenger.Application.UseCases;

/// <summary>
/// FASE 7/8 (examen): selecciona un asiento DISPONIBLE por vuelo y clase,
/// valida reglas críticas y deja el asiento en estado RESERVADO asociado al pasajero.
/// Transacción única en MySQL: <c>seats</c>, <c>disponibilidad_asientos</c> y pasajero quedan alineados o se revierte todo.
/// </summary>
public sealed class SelectSeatForPassengerUseCase
{
    private readonly IBookingPassengerRepository _passengerRepository;
    private readonly IBookingRepository          _bookingRepository;
    private readonly ISeatAvailabilityRepository _seatAvailabilityRepository;
    private readonly ISeatRepository             _seatRepository;
    private readonly AppDbContext                 _db;

    public SelectSeatForPassengerUseCase(
        IBookingPassengerRepository passengerRepository,
        IBookingRepository          bookingRepository,
        ISeatAvailabilityRepository seatAvailabilityRepository,
        ISeatRepository             seatRepository,
        AppDbContext                  db)
    {
        _passengerRepository        = passengerRepository;
        _bookingRepository          = bookingRepository;
        _seatAvailabilityRepository = seatAvailabilityRepository;
        _seatRepository             = seatRepository;
        _db                         = db;
    }

    public Task SelectAsync(
        int pasajeroReservaId,
        int vueloId,
        int flightClassId,
        string seatNumber,
        bool joinAmbientTransaction = false,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(seatNumber))
            throw new ArgumentException("Debe indicar el número de asiento.");
        if (flightClassId <= 0)
            throw new ArgumentException("Debe indicar la clase de vuelo.");

        if (joinAmbientTransaction)
            return RunCoreAsync(pasajeroReservaId, vueloId, flightClassId, seatNumber, cancellationToken);

        return RunInNewTransactionAsync(
            pasajeroReservaId, vueloId, flightClassId, seatNumber, cancellationToken);
    }

    private async Task RunInNewTransactionAsync(
        int pasajeroReservaId,
        int vueloId,
        int flightClassId,
        string seatNumber,
        CancellationToken cancellationToken)
    {
        // Estrategia de reintentos: misma conexión/tx que en SeedMySQL
        var strategy = _db.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            await using var t = await _db.Database.BeginTransactionAsync(cancellationToken);
            try
            {
                await RunCoreAsync(
                    pasajeroReservaId, vueloId, flightClassId, seatNumber, cancellationToken);
                await t.CommitAsync(cancellationToken);
            }
            catch
            {
                await t.RollbackAsync(cancellationToken);
                throw;
            }
        });
    }

    private async Task RunCoreAsync(
        int pasajeroReservaId,
        int vueloId,
        int flightClassId,
        string seatNumber,
        CancellationToken cancellationToken)
    {
        _ = cancellationToken; // repositorios actuales no propagan token en todas las capas

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

        if (booking.VueloId != vueloId)
            throw new InvalidOperationException(
                "El vuelo seleccionado no coincide con el vuelo de la reserva.");

        var normalized = seatNumber.Trim().ToUpperInvariant();
        var available = await _seatRepository.FindAvailableDetailsByFlightAndClassAsync(vueloId, flightClassId);
        var seat = available.FirstOrDefault(s =>
            string.Equals(s.SeatNumber, normalized, StringComparison.OrdinalIgnoreCase));

        if (seat is null)
            throw new InvalidOperationException(
                "El asiento no existe, no pertenece a la clase seleccionada, " +
                "o no está disponible para este vuelo (tabla seats).");

        if (!await _seatRepository.TryReserveAsync(seat.Id))
            throw new InvalidOperationException(
                "El asiento ya no está disponible (puede estar Reserved, Occupied o Blocked).");

        await _seatRepository.SetBookingIdAsync(seat.Id, booking.Id);

        var dispDisponibles = await _seatAvailabilityRepository.FindDetallesByVueloAsync(vueloId, estado: "DISPONIBLE");
        var disp = dispDisponibles.FirstOrDefault(d =>
            string.Equals(d.NumeroAsiento, normalized, StringComparison.OrdinalIgnoreCase));
        if (disp is null)
            throw new InvalidOperationException(
                "Se reservó en tabla seats, pero no se encontró la disponibilidad correspondiente (disponibilidad_asientos).");

        if (!await _seatAvailabilityRepository.TryReserveDisponibilidadAsync(disp.DisponibilidadId))
            throw new InvalidOperationException(
                "Se reservó en tabla seats, pero la disponibilidad ya no está DISPONIBLE.");

        await _seatAvailabilityRepository.SetReservaIdAsync(disp.DisponibilidadId, booking.Id);

        passenger.AsignarAsiento(disp.DisponibilidadId);
        await _passengerRepository.UpdateAsync(passenger);
    }
}
