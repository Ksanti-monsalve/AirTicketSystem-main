// src/modules/seatavailability/Application/UseCases/GetAvailableSeatsByFlightUseCase.cs
using AirTicketSystem.modules.seatavailability.Domain.aggregate;
using AirTicketSystem.modules.seatavailability.Domain.Repositories;

namespace AirTicketSystem.modules.seatavailability.Application.UseCases;

public sealed class GetAvailableSeatsByFlightUseCase
{
    private readonly ISeatAvailabilityRepository _repository;

    public GetAvailableSeatsByFlightUseCase(ISeatAvailabilityRepository repository)
        => _repository = repository;

    public async Task<IReadOnlyCollection<SeatAvailability>> ExecuteAsync(
        int vueloId, CancellationToken cancellationToken = default)
    {
        if (vueloId <= 0)
            throw new ArgumentException("El ID del vuelo no es válido.");

        return await _repository.FindDisponiblesByVueloAsync(vueloId);
    }
}
