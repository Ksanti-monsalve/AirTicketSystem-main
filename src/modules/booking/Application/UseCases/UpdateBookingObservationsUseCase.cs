// src/modules/booking/Application/UseCases/UpdateBookingObservationsUseCase.cs
using AirTicketSystem.modules.booking.Domain.aggregate;
using AirTicketSystem.modules.booking.Domain.Repositories;

namespace AirTicketSystem.modules.booking.Application.UseCases;

public sealed class UpdateBookingObservationsUseCase
{
    private readonly IBookingRepository _repository;

    public UpdateBookingObservationsUseCase(IBookingRepository repository) => _repository = repository;

    public async Task<Booking> ExecuteAsync(
        int id, string? observaciones, CancellationToken cancellationToken = default)
    {
        if (id <= 0)
            throw new ArgumentException("El ID de la reserva no es válido.");

        var booking = await _repository.FindByIdAsync(id)
            ?? throw new KeyNotFoundException($"No se encontró una reserva con ID {id}.");

        booking.ActualizarObservaciones(observaciones);
        await _repository.UpdateAsync(booking);
        return booking;
    }
}
