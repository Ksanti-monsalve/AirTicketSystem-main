// src/modules/booking/Application/UseCases/GetBookingsByClienteUseCase.cs
using AirTicketSystem.modules.booking.Domain.aggregate;
using AirTicketSystem.modules.booking.Domain.Repositories;

namespace AirTicketSystem.modules.booking.Application.UseCases;

public sealed class GetBookingsByClienteUseCase
{
    private readonly IBookingRepository _repository;

    public GetBookingsByClienteUseCase(IBookingRepository repository) => _repository = repository;

    public async Task<IReadOnlyCollection<Booking>> ExecuteAsync(
        int clienteId, CancellationToken cancellationToken = default)
    {
        if (clienteId <= 0)
            throw new ArgumentException("El ID del cliente no es válido.");

        return await _repository.FindByClienteAsync(clienteId);
    }
}
