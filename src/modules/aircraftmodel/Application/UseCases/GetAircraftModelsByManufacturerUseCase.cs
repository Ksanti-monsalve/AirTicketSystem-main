// src/modules/aircraftmodel/Application/UseCases/GetAircraftModelsByManufacturerUseCase.cs
using AirTicketSystem.modules.aircraftmodel.Domain.Repositories;
using AirTicketSystem.modules.aircraftmodel.Domain.aggregate;

namespace AirTicketSystem.modules.aircraftmodel.Application.UseCases;

public class GetAircraftModelsByManufacturerUseCase
{
    private readonly IAircraftModelRepository _repository;

    public GetAircraftModelsByManufacturerUseCase(IAircraftModelRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyCollection<AircraftModel>> ExecuteAsync(
        int fabricanteId,
        CancellationToken cancellationToken = default)
    {
        if (fabricanteId <= 0)
            throw new ArgumentException("El ID del fabricante no es válido.");

        return await _repository.FindByFabricanteAsync(fabricanteId);
    }
}