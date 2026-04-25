// src/modules/airport/Application/UseCases/GetAirportsByCityUseCase.cs
using AirTicketSystem.modules.airport.Domain.Repositories;
using AirTicketSystem.modules.airport.Domain.aggregate;

namespace AirTicketSystem.modules.airport.Application.UseCases;

public class GetAirportsByCityUseCase
{
    private readonly IAirportRepository _repository;

    public GetAirportsByCityUseCase(IAirportRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyCollection<Airport>> ExecuteAsync(
        int ciudadId,
        CancellationToken cancellationToken = default)
    {
        if (ciudadId <= 0)
            throw new ArgumentException("El ID de la ciudad no es válido.");

        return await _repository.FindByCiudadAsync(ciudadId);
    }
}