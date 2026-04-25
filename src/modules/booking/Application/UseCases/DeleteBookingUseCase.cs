// src/modules/booking/Application/UseCases/DeleteBookingUseCase.cs
using AirTicketSystem.modules.booking.Domain.Repositories;

namespace AirTicketSystem.modules.booking.Application.UseCases;

public sealed class DeleteBookingUseCase
{
    private readonly IBookingRepository _repository;

    public DeleteBookingUseCase(IBookingRepository repository) => _repository = repository;

    public async Task ExecuteAsync(int id, CancellationToken cancellationToken = default)
    {
        if (id <= 0)
            throw new ArgumentException("El ID de la reserva no es válido.");

        var booking = await _repository.FindByIdAsync(id)
            ?? throw new KeyNotFoundException($"No se encontró una reserva con ID {id}.");

        if (booking.EstaActiva)
            throw new InvalidOperationException(
                "No se puede eliminar una reserva activa. Cancélela primero.");

        await _repository.DeleteAsync(id);
    }
}
