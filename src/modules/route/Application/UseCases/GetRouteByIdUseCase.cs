// src/modules/route/Application/UseCases/GetRouteByIdUseCase.cs
using AirTicketSystem.modules.route.Domain.Repositories;
using AirTicketSystem.modules.route.Domain.aggregate;

namespace AirTicketSystem.modules.route.Application.UseCases;

public class GetRouteByIdUseCase
{
    private readonly IRouteRepository _repository;

    public GetRouteByIdUseCase(IRouteRepository repository)
    {
        _repository = repository;
    }

    public async Task<Route> ExecuteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        if (id <= 0)
            throw new ArgumentException("El ID de la ruta no es válido.");

        return await _repository.FindByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró una ruta con ID {id}.");
    }
}