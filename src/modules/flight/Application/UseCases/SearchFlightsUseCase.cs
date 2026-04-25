// src/modules/flight/Application/UseCases/SearchFlightsUseCase.cs
using AirTicketSystem.modules.flight.Domain.Repositories;
using AirTicketSystem.modules.flight.Domain.aggregate;
using AirTicketSystem.modules.airport.Domain.Repositories;

namespace AirTicketSystem.modules.flight.Application.UseCases;

public sealed class SearchFlightsUseCase
{
    private readonly IFlightRepository _flightRepository;
    private readonly IAirportRepository _airportRepository;

    public SearchFlightsUseCase(
        IFlightRepository flightRepository,
        IAirportRepository airportRepository)
    {
        _flightRepository  = flightRepository;
        _airportRepository = airportRepository;
    }

    public async Task<IReadOnlyCollection<Flight>> ExecuteAsync(
        int origenId,
        int destinoId,
        DateTime fecha,
        CancellationToken cancellationToken = default)
    {
        if (origenId <= 0)
            throw new ArgumentException(
                "El ID del aeropuerto de origen no es válido.");

        if (destinoId <= 0)
            throw new ArgumentException(
                "El ID del aeropuerto de destino no es válido.");

        if (origenId == destinoId)
            throw new InvalidOperationException(
                "El aeropuerto de origen y destino no pueden ser el mismo.");

        if (fecha.Date < DateTime.Today)
            throw new InvalidOperationException(
                "No se pueden buscar vuelos en fechas pasadas.");

        _ = await _airportRepository.FindByIdAsync(origenId)
            ?? throw new KeyNotFoundException(
                $"No se encontró el aeropuerto de origen con ID {origenId}.");

        _ = await _airportRepository.FindByIdAsync(destinoId)
            ?? throw new KeyNotFoundException(
                $"No se encontró el aeropuerto de destino con ID {destinoId}.");

        var vuelos = await _flightRepository
            .FindByOrigenAndDestinoAsync(origenId, destinoId, fecha);

        if (!vuelos.Any())
            throw new InvalidOperationException(
                "No se encontraron vuelos disponibles para el origen, " +
                "destino y fecha seleccionados.");

        return vuelos;
    }
}