// src/modules/flight/Application/UseCases/StartBoardingUseCase.cs
using AirTicketSystem.modules.flight.Domain.Repositories;
using AirTicketSystem.modules.flightcrew.Domain.Repositories;

namespace AirTicketSystem.modules.flight.Application.UseCases;

public sealed class StartBoardingUseCase
{
    private readonly IFlightRepository _flightRepository;
    private readonly IFlightCrewRepository _crewRepository;

    public StartBoardingUseCase(
        IFlightRepository flightRepository,
        IFlightCrewRepository crewRepository)
    {
        _flightRepository = flightRepository;
        _crewRepository   = crewRepository;
    }

    public async Task ExecuteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var flight = await _flightRepository.FindByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un vuelo con ID {id}.");

        if (!await _crewRepository.VueloTienePilotoAsync(id))
            throw new InvalidOperationException(
                "No se puede iniciar el abordaje: el vuelo no tiene piloto asignado.");

        if (!await _crewRepository.VueloTieneCopiloAsync(id))
            throw new InvalidOperationException(
                "No se puede iniciar el abordaje: el vuelo no tiene copiloto asignado.");

        // El aggregate valida el estado y la puerta
        flight.IniciarAbordaje();

        await _flightRepository.UpdateAsync(flight);
    }
}