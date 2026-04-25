// src/modules/flight/Application/UseCases/DeleteFlightUseCase.cs
using AirTicketSystem.modules.flight.Domain.Repositories;

namespace AirTicketSystem.modules.flight.Application.UseCases;

public sealed class DeleteFlightUseCase
{
    private readonly IFlightRepository _repository;

    public DeleteFlightUseCase(IFlightRepository repository)
    {
        _repository = repository;
    }

    public async Task ExecuteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var flight = await _repository.FindByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un vuelo con ID {id}.");

        if (flight.Estado.Valor == "EN_VUELO" ||
            flight.Estado.Valor == "ABORDANDO")
            throw new InvalidOperationException(
                $"No se puede eliminar un vuelo en estado '{flight.Estado}'.");

        if (flight.Estado.Valor == "ATERRIZADO")
            throw new InvalidOperationException(
                "No se puede eliminar un vuelo que ya aterrizó. " +
                "Los vuelos completados forman parte del historial.");

        await _repository.DeleteAsync(id);
    }
}