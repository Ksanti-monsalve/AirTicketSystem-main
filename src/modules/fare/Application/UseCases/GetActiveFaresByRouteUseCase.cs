// src/modules/fare/Application/UseCases/GetActiveFaresByRouteUseCase.cs
using AirTicketSystem.modules.fare.Domain.aggregate;
using AirTicketSystem.modules.fare.Domain.Repositories;

namespace AirTicketSystem.modules.fare.Application.UseCases;

public sealed class GetActiveFaresByRouteUseCase
{
    private readonly IFareRepository _repository;

    public GetActiveFaresByRouteUseCase(IFareRepository repository) => _repository = repository;

    public async Task<IReadOnlyCollection<Fare>> ExecuteAsync(int rutaId, CancellationToken cancellationToken = default)
    {
        if (rutaId <= 0)
            throw new ArgumentException("El ID de la ruta no es válido.");

        return await _repository.FindActivasByRutaAsync(rutaId);
    }
}
