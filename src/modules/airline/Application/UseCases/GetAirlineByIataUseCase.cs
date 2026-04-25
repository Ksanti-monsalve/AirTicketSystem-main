// src/modules/airline/Application/UseCases/GetAirlineByIataUseCase.cs
using AirTicketSystem.modules.airline.Domain.Repositories;
using AirTicketSystem.modules.airline.Domain.aggregate;
using AirTicketSystem.modules.airline.Domain.ValueObjects;

namespace AirTicketSystem.modules.airline.Application.UseCases;

public class GetAirlineByIataUseCase
{
    private readonly IAirlineRepository _repository;

    public GetAirlineByIataUseCase(IAirlineRepository repository)
    {
        _repository = repository;
    }

    public async Task<Airline> ExecuteAsync(
        string codigoIata,
        CancellationToken cancellationToken = default)
    {
        var iataVO = CodigoIataAerolinea.Crear(codigoIata);

        return await _repository.FindByCodigoIataAsync(iataVO.Valor)
            ?? throw new KeyNotFoundException(
                $"No se encontró una aerolínea con código IATA '{iataVO.Valor}'.");
    }
}