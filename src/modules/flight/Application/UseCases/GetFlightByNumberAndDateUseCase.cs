// src/modules/flight/Application/UseCases/GetFlightByNumberAndDateUseCase.cs
using AirTicketSystem.modules.flight.Domain.Repositories;
using AirTicketSystem.modules.flight.Domain.aggregate;

namespace AirTicketSystem.modules.flight.Application.UseCases;

public sealed class GetFlightByNumberAndDateUseCase
{
    private readonly IFlightRepository _repository;

    public GetFlightByNumberAndDateUseCase(IFlightRepository repository)
    {
        _repository = repository;
    }

    public async Task<Flight> ExecuteAsync(
        string numeroVuelo,
        DateTime fecha,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(numeroVuelo))
            throw new ArgumentException("El número de vuelo es obligatorio.");

        return await _repository.FindByNumeroVueloAndFechaAsync(
            numeroVuelo.ToUpperInvariant(), fecha)
            ?? throw new KeyNotFoundException(
                $"No se encontró el vuelo '{numeroVuelo}' " +
                $"para la fecha {fecha:dd/MM/yyyy}.");
    }
}