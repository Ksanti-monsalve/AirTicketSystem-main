// src/modules/booking/Application/UseCases/GetBookingByIdUseCase.cs
using AirTicketSystem.modules.booking.Domain.aggregate;
using AirTicketSystem.modules.booking.Domain.Repositories;

namespace AirTicketSystem.modules.booking.Application.UseCases;

public sealed class GetBookingByIdUseCase
{
    private readonly IBookingRepository _repository;

    public GetBookingByIdUseCase(IBookingRepository repository) => _repository = repository;

    public async Task<Booking> ExecuteAsync(int id, CancellationToken cancellationToken = default)
    {
        if (id <= 0)
            throw new ArgumentException("El ID de la reserva no es válido.");

        return await _repository.FindByIdAsync(id)
            ?? throw new KeyNotFoundException($"No se encontró una reserva con ID {id}.");
    }
}
