// src/modules/fare/Application/UseCases/GetFaresByRouteUseCase.cs
using AirTicketSystem.modules.fare.Domain.aggregate;
using AirTicketSystem.modules.fare.Domain.Repositories;

namespace AirTicketSystem.modules.fare.Application.UseCases;

public sealed class GetFaresByRouteUseCase
{
    private readonly IFareRepository _repository;

    public GetFaresByRouteUseCase(IFareRepository repository) => _repository = repository;

    public async Task<IReadOnlyCollection<Fare>> ExecuteAsync(int rutaId, CancellationToken cancellationToken = default)
    {
        if (rutaId <= 0)
            throw new ArgumentException("El ID de la ruta no es válido.");

        return await _repository.FindByRutaAsync(rutaId);
    }
}
