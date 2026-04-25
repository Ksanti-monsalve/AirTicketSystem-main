// src/modules/route/Application/UseCases/UpdateRouteUseCase.cs
using AirTicketSystem.modules.route.Domain.Repositories;
using AirTicketSystem.modules.route.Domain.aggregate;

namespace AirTicketSystem.modules.route.Application.UseCases;

public class UpdateRouteUseCase
{
    private readonly IRouteRepository _repository;

    public UpdateRouteUseCase(IRouteRepository repository)
    {
        _repository = repository;
    }

    public async Task<Route> ExecuteAsync(
        int id,
        int? distanciaKm,
        int? duracionMin,
        CancellationToken cancellationToken = default)
    {
        var ruta = await _repository.FindByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró una ruta con ID {id}.");

        ruta.ActualizarDistancia(distanciaKm);
        ruta.ActualizarDuracion(duracionMin);

        await _repository.UpdateAsync(ruta);
        return ruta;
    }
}