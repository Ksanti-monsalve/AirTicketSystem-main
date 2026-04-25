// src/modules/flight/Application/UseCases/StartFlightUseCase.cs
using AirTicketSystem.modules.flight.Domain.Repositories;
using AirTicketSystem.modules.aircraft.Domain.Repositories;

namespace AirTicketSystem.modules.flight.Application.UseCases;

public sealed class StartFlightUseCase
{
    private readonly IFlightRepository _flightRepository;
    private readonly IAircraftRepository _aircraftRepository;

    public StartFlightUseCase(
        IFlightRepository flightRepository,
        IAircraftRepository aircraftRepository)
    {
        _flightRepository   = flightRepository;
        _aircraftRepository = aircraftRepository;
    }

    public async Task ExecuteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var flight = await _flightRepository.FindByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un vuelo con ID {id}.");

        var avion = await _aircraftRepository.FindByIdAsync(flight.AvionId)
            ?? throw new KeyNotFoundException(
                "No se encontró el avión asociado al vuelo.");

        // El aggregate del vuelo valida el estado
        flight.IniciarVuelo();

        // El aggregate del avión cambia su estado a EN_VUELO
        avion.AsignarAVuelo();

        await _flightRepository.UpdateAsync(flight);
        await _aircraftRepository.UpdateAsync(avion);
    }
}