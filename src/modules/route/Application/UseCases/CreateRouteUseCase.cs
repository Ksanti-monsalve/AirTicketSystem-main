// src/modules/route/Application/UseCases/CreateRouteUseCase.cs
using AirTicketSystem.modules.route.Domain.Repositories;
using AirTicketSystem.modules.route.Domain.aggregate;
using AirTicketSystem.modules.airline.Domain.Repositories;
using AirTicketSystem.modules.airport.Domain.Repositories;

namespace AirTicketSystem.modules.route.Application.UseCases;

public sealed class CreateRouteUseCase
{
    private readonly IRouteRepository   _repository;
    private readonly IAirlineRepository _airlineRepository;
    private readonly IAirportRepository _airportRepository;

    public CreateRouteUseCase(
        IRouteRepository   repository,
        IAirlineRepository airlineRepository,
        IAirportRepository airportRepository)
    {
        _repository        = repository;
        _airlineRepository = airlineRepository;
        _airportRepository = airportRepository;
    }

    public async Task<Route> ExecuteAsync(
        int aerolineaId,
        int origenId,
        int destinoId,
        int? distanciaKm,
        int? duracionMin,
        CancellationToken cancellationToken = default)
    {
        var aerolinea = await _airlineRepository.FindByIdAsync(aerolineaId)
            ?? throw new KeyNotFoundException(
                $"No se encontró una aerolínea con ID {aerolineaId}.");

        if (!aerolinea.Activa.Valor)
            throw new InvalidOperationException(
                $"La aerolínea '{aerolinea.Nombre.Valor}' está inactiva. " +
                "No se pueden crear rutas para aerolíneas inactivas.");

        var origen = await _airportRepository.FindByIdAsync(origenId)
            ?? throw new KeyNotFoundException(
                $"No se encontró un aeropuerto de origen con ID {origenId}.");

        var destino = await _airportRepository.FindByIdAsync(destinoId)
            ?? throw new KeyNotFoundException(
                $"No se encontró un aeropuerto de destino con ID {destinoId}.");

        if (origenId == destinoId)
            throw new InvalidOperationException(
                "El aeropuerto de origen y destino no pueden ser el mismo.");

        if (!origen.Activo.Valor)
            throw new InvalidOperationException(
                $"El aeropuerto de origen '{origen.Nombre.Valor}' está inactivo.");

        if (!destino.Activo.Valor)
            throw new InvalidOperationException(
                $"El aeropuerto de destino '{destino.Nombre.Valor}' está inactivo.");

        if (await _repository.ExistsByAerolineaOrigenDestinoAsync(
            aerolineaId, origenId, destinoId))
            throw new InvalidOperationException(
                $"Ya existe una ruta de '{origen.Nombre.Valor}' a " +
                $"'{destino.Nombre.Valor}' para la aerolínea " +
                $"'{aerolinea.Nombre.Valor}'.");

        var route = Route.Crear(
            aerolineaId, origenId, destinoId, distanciaKm, duracionMin);

        await _repository.SaveAsync(route);
        return route;
    }
}
