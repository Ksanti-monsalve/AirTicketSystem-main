// src/modules/flight/Application/UseCases/OpenCheckinUseCase.cs
using AirTicketSystem.modules.flight.Domain.Repositories;

namespace AirTicketSystem.modules.flight.Application.UseCases;

public sealed class OpenCheckinUseCase
{
    private readonly IFlightRepository _repository;

    public OpenCheckinUseCase(IFlightRepository repository)
    {
        _repository = repository;
    }

    public async Task ExecuteAsync(
        int id,
        DateTime apertura,
        DateTime cierre,
        CancellationToken cancellationToken = default)
    {
        var flight = await _repository.FindByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un vuelo con ID {id}.");

        // El aggregate valida el estado y las fechas
        flight.AbrirCheckin(apertura, cierre);

        await _repository.UpdateAsync(flight);
    }
}