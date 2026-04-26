using AirTicketSystem.modules.seatavailability.Domain.Repositories;

namespace AirTicketSystem.modules.seatavailability.Application.UseCases;

public sealed class GetAvailableClassesByFlightUseCase
{
    private readonly ISeatAvailabilityRepository _repository;

    public GetAvailableClassesByFlightUseCase(ISeatAvailabilityRepository repository)
        => _repository = repository;

    public async Task<IReadOnlyCollection<ServiceClassAvailability>> ExecuteAsync(
        int vueloId,
        CancellationToken cancellationToken = default)
    {
        if (vueloId <= 0)
            throw new ArgumentException("El ID del vuelo no es válido.");

        return await _repository.FindClasesDisponiblesByVueloAsync(vueloId);
    }
}

