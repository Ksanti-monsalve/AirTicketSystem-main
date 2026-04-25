// src/modules/flight/Application/UseCases/RegisterLandingFlightUseCase.cs
using AirTicketSystem.modules.flight.Domain.Repositories;
using AirTicketSystem.modules.aircraft.Domain.Repositories;

namespace AirTicketSystem.modules.flight.Application.UseCases;

public sealed class RegisterLandingFlightUseCase
{
    private readonly IFlightRepository _flightRepository;
    private readonly IAircraftRepository _aircraftRepository;

    public RegisterLandingFlightUseCase(
        IFlightRepository flightRepository,
        IAircraftRepository aircraftRepository)
    {
        _flightRepository   = flightRepository;
        _aircraftRepository = aircraftRepository;
    }

    public async Task ExecuteAsync(
        int id,
        DateTime fechaLlegadaReal,
        CancellationToken cancellationToken = default)
    {
        var flight = await _flightRepository.FindByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un vuelo con ID {id}.");

        var avion = await _aircraftRepository.FindByIdAsync(flight.AvionId)
            ?? throw new KeyNotFoundException(
                "No se encontró el avión asociado al vuelo.");

        var duracionHoras = (decimal)(fechaLlegadaReal - flight.FechaSalida.Valor)
            .TotalHours;

        // Ambos aggregates manejan su propio estado
        flight.RegistrarAterrizaje(fechaLlegadaReal);
        avion.RegistrarAterrizaje(duracionHoras);

        await _flightRepository.UpdateAsync(flight);
        await _aircraftRepository.UpdateAsync(avion);
    }
}