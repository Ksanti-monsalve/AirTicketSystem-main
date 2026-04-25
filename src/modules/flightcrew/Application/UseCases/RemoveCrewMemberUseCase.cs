// src/modules/flightcrew/Application/UseCases/RemoveCrewMemberUseCase.cs
using AirTicketSystem.modules.flightcrew.Domain.Repositories;
using AirTicketSystem.modules.flight.Domain.Repositories;

namespace AirTicketSystem.modules.flightcrew.Application.UseCases;

public sealed class RemoveCrewMemberUseCase
{
    private readonly IFlightCrewRepository _repository;
    private readonly IFlightRepository _flightRepository;

    public RemoveCrewMemberUseCase(
        IFlightCrewRepository repository,
        IFlightRepository flightRepository)
    {
        _repository       = repository;
        _flightRepository = flightRepository;
    }

    public async Task ExecuteAsync(
        int flightCrewId,
        CancellationToken cancellationToken = default)
    {
        var asignacion = await _repository.FindByIdAsync(flightCrewId)
            ?? throw new KeyNotFoundException(
                $"No se encontró la asignación de tripulación con ID {flightCrewId}.");

        var vuelo = await _flightRepository.FindByIdAsync(asignacion.VueloId)
            ?? throw new KeyNotFoundException(
                "No se encontró el vuelo asociado a esta tripulación.");

        if (vuelo.Estado.Valor == "EN_VUELO" ||
            vuelo.Estado.Valor == "ABORDANDO")
            throw new InvalidOperationException(
                $"No se puede remover tripulación de un vuelo en estado " +
                $"'{vuelo.Estado}'.");

        if (vuelo.Estado.Valor == "ATERRIZADO" ||
            vuelo.Estado.Valor == "CANCELADO")
            throw new InvalidOperationException(
                "No se puede modificar la tripulación de un vuelo finalizado.");

        await _repository.DeleteAsync(flightCrewId);
    }
}