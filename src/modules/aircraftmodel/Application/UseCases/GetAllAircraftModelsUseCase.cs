// src/modules/aircraftmodel/Application/UseCases/GetAllAircraftModelsUseCase.cs
using AirTicketSystem.modules.aircraftmodel.Domain.Repositories;
using AirTicketSystem.modules.aircraftmodel.Domain.aggregate;

namespace AirTicketSystem.modules.aircraftmodel.Application.UseCases;

public class GetAllAircraftModelsUseCase
{
    private readonly IAircraftModelRepository _repository;

    public GetAllAircraftModelsUseCase(IAircraftModelRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyCollection<AircraftModel>> ExecuteAsync(
        CancellationToken cancellationToken = default)
        => (await _repository.FindAllAsync())
            .OrderBy(m => m.Nombre.Valor)
            .ToList()
            .AsReadOnly();
}