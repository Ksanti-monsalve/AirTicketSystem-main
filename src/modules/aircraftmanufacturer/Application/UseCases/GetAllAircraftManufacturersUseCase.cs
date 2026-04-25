// src/modules/aircraftmanufacturer/Application/UseCases/GetAllAircraftManufacturersUseCase.cs
using AirTicketSystem.modules.aircraftmanufacturer.Domain.Repositories;
using AirTicketSystem.modules.aircraftmanufacturer.Domain.aggregate;

namespace AirTicketSystem.modules.aircraftmanufacturer.Application.UseCases;

public class GetAllAircraftManufacturersUseCase
{
    private readonly IAircraftManufacturerRepository _repository;

    public GetAllAircraftManufacturersUseCase(
        IAircraftManufacturerRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyCollection<AircraftManufacturer>> ExecuteAsync(
        CancellationToken cancellationToken = default)
        => (await _repository.FindAllAsync())
            .OrderBy(f => f.Nombre.Valor)
            .ToList()
            .AsReadOnly();
}