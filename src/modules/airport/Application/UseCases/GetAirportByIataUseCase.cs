// src/modules/airport/Application/UseCases/GetAirportByIataUseCase.cs
using AirTicketSystem.modules.airport.Domain.Repositories;
using AirTicketSystem.modules.airport.Domain.aggregate;
using AirTicketSystem.modules.airport.Domain.ValueObjects;

namespace AirTicketSystem.modules.airport.Application.UseCases;

public class GetAirportByIataUseCase
{
    private readonly IAirportRepository _repository;

    public GetAirportByIataUseCase(IAirportRepository repository)
    {
        _repository = repository;
    }

    public async Task<Airport> ExecuteAsync(
        string codigoIata,
        CancellationToken cancellationToken = default)
    {
        var iataVO = CodigoIataAirport.Crear(codigoIata);

        return await _repository.FindByCodigoIataAsync(iataVO.Valor)
            ?? throw new KeyNotFoundException(
                $"No se encontró un aeropuerto con código IATA '{iataVO.Valor}'.");
    }
}