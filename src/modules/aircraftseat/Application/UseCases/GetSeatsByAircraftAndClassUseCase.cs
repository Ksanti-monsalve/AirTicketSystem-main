// src/modules/aircraftseat/Application/UseCases/GetSeatsByAircraftAndClassUseCase.cs
using AirTicketSystem.modules.aircraftseat.Domain.Repositories;
using AirTicketSystem.modules.aircraftseat.Domain.aggregate;

namespace AirTicketSystem.modules.aircraftseat.Application.UseCases;

public class GetSeatsByAircraftAndClassUseCase
{
    private readonly IAircraftSeatRepository _repository;

    public GetSeatsByAircraftAndClassUseCase(IAircraftSeatRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyCollection<AircraftSeat>> ExecuteAsync(
        int avionId,
        int claseServicioId,
        CancellationToken cancellationToken = default)
    {
        if (avionId <= 0)
            throw new ArgumentException("El ID del avión no es válido.");

        if (claseServicioId <= 0)
            throw new ArgumentException(
                "El ID de la clase de servicio no es válido.");

        return await _repository.FindByAvionAndClaseAsync(avionId, claseServicioId);
    }
}