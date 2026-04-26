using AirTicketSystem.modules.seatavailability.Domain.Repositories;

namespace AirTicketSystem.modules.seatavailability.Application.UseCases;

public sealed class GetAvailableSeatDetailsByFlightAndClassUseCase
{
    private readonly ISeatAvailabilityRepository _repository;

    public GetAvailableSeatDetailsByFlightAndClassUseCase(ISeatAvailabilityRepository repository)
        => _repository = repository;

    public async Task<IReadOnlyCollection<SeatAvailabilityDetail>> ExecuteAsync(
        int vueloId,
        int claseServicioId,
        CancellationToken cancellationToken = default)
    {
        if (vueloId <= 0)
            throw new ArgumentException("El ID del vuelo no es válido.");
        if (claseServicioId <= 0)
            throw new ArgumentException("El ID de la clase no es válido.");

        return await _repository.FindDetallesDisponiblesByVueloAndClaseAsync(vueloId, claseServicioId);
    }
}

