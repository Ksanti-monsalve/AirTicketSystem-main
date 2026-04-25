// src/modules/aircraftseat/Application/UseCases/GetSeatsByAircraftUseCase.cs
using AirTicketSystem.modules.aircraftseat.Domain.Repositories;
using AirTicketSystem.modules.aircraftseat.Domain.aggregate;

namespace AirTicketSystem.modules.aircraftseat.Application.UseCases;

public class GetSeatsByAircraftUseCase
{
    private readonly IAircraftSeatRepository _repository;

    public GetSeatsByAircraftUseCase(IAircraftSeatRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyCollection<AircraftSeat>> ExecuteAsync(
        int avionId,
        CancellationToken cancellationToken = default)
    {
        if (avionId <= 0)
            throw new ArgumentException("El ID del avión no es válido.");

        return await _repository.FindByAvionAsync(avionId);
    }
}