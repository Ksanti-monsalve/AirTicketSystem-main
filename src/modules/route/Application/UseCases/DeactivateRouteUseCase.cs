// src/modules/route/Application/UseCases/DeactivateRouteUseCase.cs
using AirTicketSystem.modules.route.Domain.Repositories;

namespace AirTicketSystem.modules.route.Application.UseCases;

public class DeactivateRouteUseCase
{
    private readonly IRouteRepository _repository;

    public DeactivateRouteUseCase(IRouteRepository repository)
    {
        _repository = repository;
    }

    public async Task ExecuteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        var ruta = await _repository.FindByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró una ruta con ID {id}.");

        ruta.Desactivar();
        await _repository.UpdateAsync(ruta);
    }
}