// src/modules/bookingpassenger/Application/UseCases/ChangeSeatUseCase.cs
using AirTicketSystem.modules.bookingpassenger.Domain.aggregate;
using AirTicketSystem.modules.bookingpassenger.Domain.Repositories;

namespace AirTicketSystem.modules.bookingpassenger.Application.UseCases;

public sealed class ChangeSeatUseCase
{
    private readonly IBookingPassengerRepository _repository;

    public ChangeSeatUseCase(IBookingPassengerRepository repository) => _repository = repository;

    public async Task<BookingPassenger> ExecuteAsync(
        int pasajeroReservaId, int nuevoAsientoId, CancellationToken cancellationToken = default)
    {
        var passenger = await _repository.FindByIdAsync(pasajeroReservaId)
            ?? throw new KeyNotFoundException(
                $"No se encontró un pasajero de reserva con ID {pasajeroReservaId}.");

        passenger.CambiarAsiento(nuevoAsientoId);
        await _repository.UpdateAsync(passenger);
        return passenger;
    }
}
