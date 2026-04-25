// src/modules/airline/Application/UseCases/CreateAirlineUseCase.cs
using AirTicketSystem.modules.airline.Domain.Repositories;
using AirTicketSystem.modules.airline.Domain.aggregate;
using AirTicketSystem.modules.airline.Domain.ValueObjects;
using AirTicketSystem.modules.country.Domain.Repositories;

namespace AirTicketSystem.modules.airline.Application.UseCases;

public sealed class CreateAirlineUseCase
{
    private readonly IAirlineRepository _repository;
    private readonly ICountryRepository _countryRepository;

    public CreateAirlineUseCase(
        IAirlineRepository repository,
        ICountryRepository countryRepository)
    {
        _repository        = repository;
        _countryRepository = countryRepository;
    }

    public async Task<Airline> ExecuteAsync(
        int paisId,
        string codigoIata,
        string codigoIcao,
        string nombre,
        string? nombreComercial,
        string? sitioWeb,
        CancellationToken cancellationToken = default)
    {
        _ = await _countryRepository.FindByIdAsync(paisId)
            ?? throw new KeyNotFoundException(
                $"No se encontró un país con ID {paisId}.");

        var iataVO   = CodigoIataAerolinea.Crear(codigoIata);
        var icaoVO   = CodigoIcaoAerolinea.Crear(codigoIcao);
        var nombreVO = NombreAerolinea.Crear(nombre);

        if (await _repository.ExistsByCodigoIataAsync(iataVO.Valor))
            throw new InvalidOperationException(
                $"Ya existe una aerolínea con el código IATA '{iataVO.Valor}'.");

        if (await _repository.ExistsByCodigoIcaoAsync(icaoVO.Valor))
            throw new InvalidOperationException(
                $"Ya existe una aerolínea con el código ICAO '{icaoVO.Valor}'.");

        var airline = Airline.Crear(
            paisId,
            iataVO.Valor,
            icaoVO.Valor,
            nombreVO.Valor,
            nombreComercial,
            sitioWeb);

        await _repository.SaveAsync(airline);
        return airline;
    }
}