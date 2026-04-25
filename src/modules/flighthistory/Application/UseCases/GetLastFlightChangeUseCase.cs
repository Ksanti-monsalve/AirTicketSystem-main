// src/modules/flighthistory/Application/UseCases/GetLastFlightChangeUseCase.cs
using AirTicketSystem.modules.flighthistory.Domain.aggregate;
using AirTicketSystem.modules.flighthistory.Domain.Repositories;

namespace AirTicketSystem.modules.flighthistory.Application.UseCases;

public sealed class GetLastFlightChangeUseCase
{
    private readonly IFlightHistoryRepository _repository;

    public GetLastFlightChangeUseCase(IFlightHistoryRepository repository) => _repository = repository;

    public async Task<FlightHistory?> ExecuteAsync(
        int vueloId, CancellationToken cancellationToken = default)
    {
        if (vueloId <= 0)
            throw new ArgumentException("El ID del vuelo no es válido.");

        return await _repository.FindUltimoCambioByVueloAsync(vueloId);
    }
}
