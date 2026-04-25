// src/modules/aircraftmanufacturer/Application/UseCases/GetAircraftManufacturersByCountryUseCase.cs
using AirTicketSystem.modules.aircraftmanufacturer.Domain.Repositories;
using AirTicketSystem.modules.aircraftmanufacturer.Domain.aggregate;

namespace AirTicketSystem.modules.aircraftmanufacturer.Application.UseCases;

public class GetAircraftManufacturersByCountryUseCase
{
    private readonly IAircraftManufacturerRepository _repository;

    public GetAircraftManufacturersByCountryUseCase(
        IAircraftManufacturerRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyCollection<AircraftManufacturer>> ExecuteAsync(
        int paisId,
        CancellationToken cancellationToken = default)
    {
        if (paisId <= 0)
            throw new ArgumentException("El ID del país no es válido.");

        return await _repository.FindByPaisAsync(paisId);
    }
}