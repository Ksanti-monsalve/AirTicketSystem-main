// src/modules/route/Application/UseCases/SearchRoutesUseCase.cs
using AirTicketSystem.modules.route.Domain.Repositories;
using AirTicketSystem.modules.route.Domain.aggregate;

namespace AirTicketSystem.modules.route.Application.UseCases;

public class SearchRoutesUseCase
{
    private readonly IRouteRepository _repository;

    public SearchRoutesUseCase(IRouteRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyCollection<Route>> ExecuteAsync(
        int origenId,
        int destinoId,
        CancellationToken cancellationToken = default)
    {
        if (origenId <= 0)
            throw new ArgumentException("El ID del aeropuerto de origen no es válido.");

        if (destinoId <= 0)
            throw new ArgumentException("El ID del aeropuerto de destino no es válido.");

        if (origenId == destinoId)
            throw new InvalidOperationException(
                "El origen y destino no pueden ser el mismo aeropuerto.");

        return await _repository.FindByOrigenAndDestinoAsync(origenId, destinoId);
    }
}