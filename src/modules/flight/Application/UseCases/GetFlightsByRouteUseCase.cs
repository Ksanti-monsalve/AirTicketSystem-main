// src/modules/flight/Application/UseCases/GetFlightsByRouteUseCase.cs
using AirTicketSystem.modules.flight.Domain.Repositories;
using AirTicketSystem.modules.flight.Domain.aggregate;

namespace AirTicketSystem.modules.flight.Application.UseCases;

public sealed class GetFlightsByRouteUseCase
{
    private readonly IFlightRepository _repository;

    public GetFlightsByRouteUseCase(IFlightRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyCollection<Flight>> ExecuteAsync(
        int rutaId,
        CancellationToken cancellationToken = default)
    {
        if (rutaId <= 0)
            throw new ArgumentException("El ID de la ruta no es válido.");

        return await _repository.FindByRutaAsync(rutaId);
    }
}