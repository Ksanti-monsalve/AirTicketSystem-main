// src/modules/route/Application/UseCases/DeleteRouteUseCase.cs
using AirTicketSystem.modules.route.Domain.Repositories;

namespace AirTicketSystem.modules.route.Application.UseCases;

public class DeleteRouteUseCase
{
    private readonly IRouteRepository _repository;

    public DeleteRouteUseCase(IRouteRepository repository)
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

        if (ruta.Activa.Valor)
            throw new InvalidOperationException(
                "No se puede eliminar una ruta activa. " +
                "Desactívela primero.");

        await _repository.DeleteAsync(id);
    }
}