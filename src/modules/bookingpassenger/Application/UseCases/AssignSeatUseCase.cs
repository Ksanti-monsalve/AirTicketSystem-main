// src/modules/bookingpassenger/Application/UseCases/AssignSeatUseCase.cs
using AirTicketSystem.modules.bookingpassenger.Domain.aggregate;
using AirTicketSystem.modules.bookingpassenger.Domain.Repositories;

namespace AirTicketSystem.modules.bookingpassenger.Application.UseCases;

public sealed class AssignSeatUseCase
{
    private readonly IBookingPassengerRepository _repository;

    public AssignSeatUseCase(IBookingPassengerRepository repository) => _repository = repository;

    public async Task<BookingPassenger> ExecuteAsync(
        int pasajeroReservaId, int asientoId, CancellationToken cancellationToken = default)
    {
        var passenger = await _repository.FindByIdAsync(pasajeroReservaId)
            ?? throw new KeyNotFoundException(
                $"No se encontró un pasajero de reserva con ID {pasajeroReservaId}.");

        passenger.AsignarAsiento(asientoId);
        await _repository.UpdateAsync(passenger);
        return passenger;
    }
}
