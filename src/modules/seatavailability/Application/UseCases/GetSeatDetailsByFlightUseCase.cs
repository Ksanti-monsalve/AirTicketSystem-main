using AirTicketSystem.modules.seatavailability.Domain.Repositories;

namespace AirTicketSystem.modules.seatavailability.Application.UseCases;

public sealed class GetSeatDetailsByFlightUseCase
{
    private readonly ISeatAvailabilityRepository _repository;

    public GetSeatDetailsByFlightUseCase(ISeatAvailabilityRepository repository)
        => _repository = repository;

    public async Task<IReadOnlyCollection<SeatAvailabilityDetail>> ExecuteAsync(
        int vueloId,
        string? estado = null,
        CancellationToken cancellationToken = default)
    {
        if (vueloId <= 0)
            throw new ArgumentException("El ID del vuelo no es válido.");

        return await _repository.FindDetallesByVueloAsync(vueloId, estado);
    }
}

