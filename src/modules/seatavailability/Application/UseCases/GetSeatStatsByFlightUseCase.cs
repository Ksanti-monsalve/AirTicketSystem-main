using AirTicketSystem.modules.seatavailability.Domain.Repositories;

namespace AirTicketSystem.modules.seatavailability.Application.UseCases;

public sealed class GetSeatStatsByFlightUseCase
{
    private readonly ISeatAvailabilityRepository _repository;

    public GetSeatStatsByFlightUseCase(ISeatAvailabilityRepository repository)
        => _repository = repository;

    public async Task<IReadOnlyCollection<ServiceClassStateStats>> ExecuteAsync(
        int vueloId,
        CancellationToken cancellationToken = default)
    {
        if (vueloId <= 0)
            throw new ArgumentException("El ID del vuelo no es válido.");

        return await _repository.FindStatsByVueloAsync(vueloId);
    }
}

