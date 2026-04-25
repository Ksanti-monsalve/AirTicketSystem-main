// src/modules/airport/Application/UseCases/GetAllAirportsUseCase.cs
using AirTicketSystem.modules.airport.Domain.Repositories;
using AirTicketSystem.modules.airport.Domain.aggregate;

namespace AirTicketSystem.modules.airport.Application.UseCases;

public class GetAllAirportsUseCase
{
    private readonly IAirportRepository _repository;

    public GetAllAirportsUseCase(IAirportRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyCollection<Airport>> ExecuteAsync(
        CancellationToken cancellationToken = default)
        => (await _repository.FindAllAsync())
            .OrderBy(a => a.Nombre.Valor)
            .ToList()
            .AsReadOnly();
}