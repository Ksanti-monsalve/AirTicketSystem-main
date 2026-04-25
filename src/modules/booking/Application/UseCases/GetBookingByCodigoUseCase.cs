// src/modules/booking/Application/UseCases/GetBookingByCodigoUseCase.cs
using AirTicketSystem.modules.booking.Domain.aggregate;
using AirTicketSystem.modules.booking.Domain.Repositories;

namespace AirTicketSystem.modules.booking.Application.UseCases;

public sealed class GetBookingByCodigoUseCase
{
    private readonly IBookingRepository _repository;

    public GetBookingByCodigoUseCase(IBookingRepository repository) => _repository = repository;

    public async Task<Booking> ExecuteAsync(
        string codigoReserva, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(codigoReserva))
            throw new ArgumentException("El código de reserva no puede estar vacío.");

        return await _repository.FindByCodigoReservaAsync(codigoReserva)
            ?? throw new KeyNotFoundException(
                $"No se encontró una reserva con código '{codigoReserva}'.");
    }
}
