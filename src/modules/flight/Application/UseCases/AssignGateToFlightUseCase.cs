// src/modules/flight/Application/UseCases/AssignGateToFlightUseCase.cs
using AirTicketSystem.modules.flight.Domain.Repositories;
using AirTicketSystem.modules.gate.Domain.Repositories;

namespace AirTicketSystem.modules.flight.Application.UseCases;

public sealed class AssignGateToFlightUseCase
{
    private readonly IFlightRepository _flightRepository;
    private readonly IGateRepository _gateRepository;

    public AssignGateToFlightUseCase(
        IFlightRepository flightRepository,
        IGateRepository gateRepository)
    {
        _flightRepository = flightRepository;
        _gateRepository   = gateRepository;
    }

    public async Task ExecuteAsync(
        int vueloId,
        int puertaEmbarqueId,
        CancellationToken cancellationToken = default)
    {
        var flight = await _flightRepository.FindByIdAsync(vueloId)
            ?? throw new KeyNotFoundException(
                $"No se encontró un vuelo con ID {vueloId}.");

        var puerta = await _gateRepository.FindByIdAsync(puertaEmbarqueId)
            ?? throw new KeyNotFoundException(
                $"No se encontró una puerta con ID {puertaEmbarqueId}.");

        if (!puerta.Activa.Valor)
            throw new InvalidOperationException(
                $"La puerta '{puerta.Codigo.Valor}' está inactiva.");

        // El aggregate valida que el estado permita asignar puerta
        flight.AsignarPuerta(puertaEmbarqueId);

        await _flightRepository.UpdateAsync(flight);
    }
}