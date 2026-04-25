// src/modules/airport/Application/UseCases/CreateAirportUseCase.cs
using AirTicketSystem.modules.airport.Domain.Repositories;
using AirTicketSystem.modules.airport.Domain.aggregate;
using AirTicketSystem.modules.airport.Domain.ValueObjects;
using AirTicketSystem.modules.city.Domain.Repositories;

namespace AirTicketSystem.modules.airport.Application.UseCases;

public class CreateAirportUseCase
{
    private readonly IAirportRepository _repository;
    private readonly ICityRepository _cityRepository;

    public CreateAirportUseCase(
        IAirportRepository repository,
        ICityRepository cityRepository)
    {
        _repository     = repository;
        _cityRepository = cityRepository;
    }

    public async Task<Airport> ExecuteAsync(
        int ciudadId,
        string codigoIata,
        string codigoIcao,
        string nombre,
        string? direccion,
        CancellationToken cancellationToken = default)
    {
        _ = await _cityRepository.FindByIdAsync(ciudadId)
            ?? throw new KeyNotFoundException(
                $"No se encontró una ciudad con ID {ciudadId}.");

        var iataVO     = CodigoIataAirport.Crear(codigoIata);
        var icaoVO     = CodigoIcaoAirport.Crear(codigoIcao);
        var nombreVO   = NombreAirport.Crear(nombre);

        if (await _repository.ExistsByCodigoIataAsync(iataVO.Valor))
            throw new InvalidOperationException(
                $"Ya existe un aeropuerto con el código IATA '{iataVO.Valor}'.");

        if (await _repository.ExistsByCodigoIcaoAsync(icaoVO.Valor))
            throw new InvalidOperationException(
                $"Ya existe un aeropuerto con el código ICAO '{icaoVO.Valor}'.");

        var airport = Airport.Crear(
            ciudadId,
            iataVO.Valor,
            icaoVO.Valor,
            nombreVO.Valor,
            direccion);

        await _repository.SaveAsync(airport);
        return airport;
    }
}