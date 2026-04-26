using AirTicketSystem.modules.booking.Domain.Repositories;
using AirTicketSystem.modules.bookingpassenger.Domain.Repositories;
using AirTicketSystem.modules.seatavailability.Domain.Repositories;

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

    public SelectSeatForPassengerUseCase(
        IBookingPassengerRepository passengerRepository,
        IBookingRepository          bookingRepository,
        ISeatAvailabilityRepository seatAvailabilityRepository)
    {
        _passengerRepository        = passengerRepository;
        _bookingRepository          = bookingRepository;
        _seatAvailabilityRepository = seatAvailabilityRepository;
    }

    public async Task SelectAsync(
        int pasajeroReservaId,
        int vueloId,
        int claseServicioId,
        string numeroAsiento,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(numeroAsiento))
            throw new ArgumentException("Debe indicar el número de asiento.");

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

        // Regla: no duplicar asiento en otra reserva (estado DISPONIBLE lo garantiza)
        // y validar que el asiento existe / pertenece al vuelo / pertenece a la clase / está disponible.
        var disponibles = await _seatAvailabilityRepository
            .FindDetallesDisponiblesByVueloAndClaseAsync(vueloId, claseServicioId);

        var numeroNormalizado = numeroAsiento.Trim().ToUpperInvariant();
        var seleccionado = disponibles.FirstOrDefault(s =>
            string.Equals(s.NumeroAsiento, numeroNormalizado, StringComparison.OrdinalIgnoreCase));

        if (seleccionado is null)
            throw new InvalidOperationException(
                "El asiento no existe, no pertenece a la clase seleccionada, " +
                "o no está disponible para este vuelo.");

        // 1) Reservar asiento en forma ATÓMICA (evita doble reserva concurrente)
        var reservado = await _seatAvailabilityRepository
            .TryReserveDisponibilidadAsync(seleccionado.DisponibilidadId);

        if (!reservado)
            throw new InvalidOperationException(
                "El asiento ya no está disponible (puede estar RESERVADO, OCUPADO o BLOQUEADO).");

        // Persistencia (examen): asociar el asiento a la reserva
        await _seatAvailabilityRepository.SetReservaIdAsync(seleccionado.DisponibilidadId, booking.Id);

        // 2) Asociar a pasajero-reserva (guarda el ID de disponibilidad_asientos)
        passenger.AsignarAsiento(seleccionado.DisponibilidadId);
        await _passengerRepository.UpdateAsync(passenger);
    }
}

