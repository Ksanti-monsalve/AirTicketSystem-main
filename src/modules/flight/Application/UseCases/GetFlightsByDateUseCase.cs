// src/modules/flight/Application/UseCases/GetFlightsByDateUseCase.cs
using AirTicketSystem.modules.flight.Domain.Repositories;
using AirTicketSystem.modules.flight.Domain.aggregate;

namespace AirTicketSystem.modules.flight.Application.UseCases;

public sealed class GetFlightsByDateUseCase
{
    private readonly IFlightRepository _repository;

    public GetFlightsByDateUseCase(IFlightRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyCollection<Flight>> ExecuteAsync(
        DateTime fecha,
        CancellationToken cancellationToken = default)
    {
        if (fecha == default)
            throw new ArgumentException("La fecha no puede estar vacía.");

        return await _repository.FindByFechaAsync(fecha);
    }
}