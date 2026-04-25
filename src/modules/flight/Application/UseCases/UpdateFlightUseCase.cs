// src/modules/flight/Application/UseCases/UpdateFlightUseCase.cs
using AirTicketSystem.modules.flight.Domain.Repositories;
using AirTicketSystem.modules.flight.Domain.aggregate;
using AirTicketSystem.modules.gate.Domain.Repositories;

namespace AirTicketSystem.modules.flight.Application.UseCases;

public sealed class UpdateFlightUseCase
{
    private readonly IFlightRepository _flightRepository;
    private readonly IGateRepository _gateRepository;

    public UpdateFlightUseCase(
        IFlightRepository flightRepository,
        IGateRepository gateRepository)
    {
        _flightRepository = flightRepository;
        _gateRepository   = gateRepository;
    }

    public async Task<Flight> ExecuteAsync(
        int id,
        DateTime fechaSalida,
        DateTime fechaLlegadaEstimada,
        int? puertaEmbarqueId,
        CancellationToken cancellationToken = default)
    {
        var flight = await _flightRepository.FindByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un vuelo con ID {id}.");

        if (puertaEmbarqueId.HasValue)
        {
            var puerta = await _gateRepository.FindByIdAsync(puertaEmbarqueId.Value)
                ?? throw new KeyNotFoundException(
                    $"No se encontró una puerta con ID {puertaEmbarqueId.Value}.");

            if (!puerta.Activa.Valor)
                throw new InvalidOperationException(
                    $"La puerta '{puerta.Codigo.Valor}' está inactiva.");

            flight.AsignarPuerta(puertaEmbarqueId.Value);
        }

        // El aggregate valida que el estado permita modificación
        flight.ActualizarHorarios(fechaSalida, fechaLlegadaEstimada);

        await _flightRepository.UpdateAsync(flight);
        return flight;
    }
}