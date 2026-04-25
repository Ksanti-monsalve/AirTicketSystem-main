// src/modules/aircraft/Application/UseCases/GetAllAircraftUseCase.cs
using AirTicketSystem.modules.aircraft.Domain.Repositories;
using AirTicketSystem.modules.aircraft.Domain.aggregate;

namespace AirTicketSystem.modules.aircraft.Application.UseCases;

public class GetAllAircraftUseCase
{
    private readonly IAircraftRepository _repository;

    public GetAllAircraftUseCase(IAircraftRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyCollection<Aircraft>> ExecuteAsync(
        CancellationToken cancellationToken = default)
        => (await _repository.FindAllAsync())
            .OrderBy(a => a.Matricula.Valor)
            .ToList()
            .AsReadOnly();
}