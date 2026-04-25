// src/modules/aircraftseat/Application/UseCases/DeleteAircraftSeatUseCase.cs
using AirTicketSystem.modules.aircraftseat.Domain.Repositories;

namespace AirTicketSystem.modules.aircraftseat.Application.UseCases;

public class DeleteAircraftSeatUseCase
{
    private readonly IAircraftSeatRepository _repository;

    public DeleteAircraftSeatUseCase(IAircraftSeatRepository repository)
    {
        _repository = repository;
    }

    public async Task ExecuteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var asiento = await _repository.FindByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un asiento con ID {id}.");

        if (asiento.Activo.Valor)
            throw new InvalidOperationException(
                $"No se puede eliminar el asiento '{asiento.CodigoAsiento.Valor}' " +
                "porque está activo. Desactívelo primero.");

        await _repository.DeleteAsync(id);
    }
}