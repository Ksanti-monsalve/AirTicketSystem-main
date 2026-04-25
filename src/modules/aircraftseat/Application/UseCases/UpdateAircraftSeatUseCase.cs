// src/modules/aircraftseat/Application/UseCases/UpdateAircraftSeatUseCase.cs
using AirTicketSystem.modules.aircraftseat.Domain.Repositories;
using AirTicketSystem.modules.aircraftseat.Domain.aggregate;

namespace AirTicketSystem.modules.aircraftseat.Application.UseCases;

public class UpdateAircraftSeatUseCase
{
    private readonly IAircraftSeatRepository _repository;

    public UpdateAircraftSeatUseCase(IAircraftSeatRepository repository)
    {
        _repository = repository;
    }

    public async Task<AircraftSeat> ExecuteAsync(
        int id,
        bool esVentana,
        bool esPasillo,
        decimal costoSeleccion,
        CancellationToken cancellationToken = default)
    {
        var asiento = await _repository.FindByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un asiento con ID {id}.");

        asiento.ActualizarCondiciones(esVentana, esPasillo, costoSeleccion);

        await _repository.UpdateAsync(asiento);
        return asiento;
    }
}