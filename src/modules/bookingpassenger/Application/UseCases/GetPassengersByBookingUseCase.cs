// src/modules/bookingpassenger/Application/UseCases/GetPassengersByBookingUseCase.cs
using AirTicketSystem.modules.bookingpassenger.Domain.aggregate;
using AirTicketSystem.modules.bookingpassenger.Domain.Repositories;

namespace AirTicketSystem.modules.bookingpassenger.Application.UseCases;

public sealed class GetPassengersByBookingUseCase
{
    private readonly IBookingPassengerRepository _repository;

    public GetPassengersByBookingUseCase(IBookingPassengerRepository repository) => _repository = repository;

    public async Task<IReadOnlyCollection<BookingPassenger>> ExecuteAsync(
        int reservaId, CancellationToken cancellationToken = default)
    {
        if (reservaId <= 0)
            throw new ArgumentException("El ID de la reserva no es válido.");

        return await _repository.FindByReservaAsync(reservaId);
    }
}
