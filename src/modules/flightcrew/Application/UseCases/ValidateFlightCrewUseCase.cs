// src/modules/flightcrew/Application/UseCases/ValidateFlightCrewUseCase.cs
using AirTicketSystem.modules.flightcrew.Domain.Repositories;

namespace AirTicketSystem.modules.flightcrew.Application.UseCases;

public sealed class ValidateFlightCrewUseCase
{
    private readonly IFlightCrewRepository _repository;

    public ValidateFlightCrewUseCase(IFlightCrewRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> ExecuteAsync(
        int vueloId,
        CancellationToken cancellationToken = default)
    {
        if (vueloId <= 0)
            throw new ArgumentException("El ID del vuelo no es válido.");

        var tienePiloto   = await _repository.VueloTienePilotoAsync(vueloId);
        var tieneCopiloto = await _repository.VueloTieneCopiloAsync(vueloId);

        return tienePiloto && tieneCopiloto;
    }
}