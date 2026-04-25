// src/modules/airline/Application/UseCases/GetAllAirlinesUseCase.cs
using AirTicketSystem.modules.airline.Domain.Repositories;
using AirTicketSystem.modules.airline.Domain.aggregate;

namespace AirTicketSystem.modules.airline.Application.UseCases;

public class GetAllAirlinesUseCase
{
    private readonly IAirlineRepository _repository;

    public GetAllAirlinesUseCase(IAirlineRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyCollection<Airline>> ExecuteAsync(
        CancellationToken cancellationToken = default)
        => (await _repository.FindAllAsync())
            .OrderBy(a => a.Nombre.Valor)
            .ToList()
            .AsReadOnly();
}