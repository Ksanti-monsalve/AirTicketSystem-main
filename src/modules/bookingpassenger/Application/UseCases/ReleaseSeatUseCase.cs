// src/modules/bookingpassenger/Application/UseCases/ReleaseSeatUseCase.cs
using AirTicketSystem.modules.bookingpassenger.Domain.aggregate;
using AirTicketSystem.modules.bookingpassenger.Domain.Repositories;

namespace AirTicketSystem.modules.bookingpassenger.Application.UseCases;

public sealed class ReleaseSeatUseCase
{
    private readonly IBookingPassengerRepository _repository;

    public ReleaseSeatUseCase(IBookingPassengerRepository repository) => _repository = repository;

    public async Task<BookingPassenger> ExecuteAsync(
        int pasajeroReservaId, CancellationToken cancellationToken = default)
    {
        var passenger = await _repository.FindByIdAsync(pasajeroReservaId)
            ?? throw new KeyNotFoundException(
                $"No se encontró un pasajero de reserva con ID {pasajeroReservaId}.");

        passenger.LiberarAsiento();
        await _repository.UpdateAsync(passenger);
        return passenger;
    }
}
