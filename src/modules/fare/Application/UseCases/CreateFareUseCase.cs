// src/modules/fare/Application/UseCases/CreateFareUseCase.cs
using AirTicketSystem.modules.fare.Domain.aggregate;
using AirTicketSystem.modules.fare.Domain.Repositories;
using AirTicketSystem.modules.route.Domain.Repositories;
using AirTicketSystem.modules.serviceclass.Domain.Repositories;

namespace AirTicketSystem.modules.fare.Application.UseCases;

public sealed class CreateFareUseCase
{
    private readonly IFareRepository _repository;
    private readonly IRouteRepository _routeRepository;
    private readonly IServiceClassRepository _serviceClassRepository;

    public CreateFareUseCase(
        IFareRepository repository,
        IRouteRepository routeRepository,
        IServiceClassRepository serviceClassRepository)
    {
        _repository            = repository;
        _routeRepository       = routeRepository;
        _serviceClassRepository = serviceClassRepository;
    }

    public async Task<Fare> ExecuteAsync(
        int rutaId,
        int claseServicioId,
        string nombre,
        decimal precioBase,
        decimal impuestos,
        bool permiteCambios,
        bool permiteReembolso,
        DateOnly? vigenteHasta,
        CancellationToken cancellationToken = default)
    {
        _ = await _routeRepository.FindByIdAsync(rutaId)
            ?? throw new KeyNotFoundException(
                $"No se encontró una ruta con ID {rutaId}.");

        _ = await _serviceClassRepository.FindByIdAsync(claseServicioId)
            ?? throw new KeyNotFoundException(
                $"No se encontró una clase de servicio con ID {claseServicioId}.");

        if (await _repository.ExistsByRutaClaseNombreAsync(rutaId, claseServicioId, nombre))
            throw new InvalidOperationException(
                $"Ya existe una tarifa con el nombre '{nombre}' para esta ruta y clase de servicio.");

        var fare = Fare.Crear(
            rutaId, claseServicioId, nombre,
            precioBase, impuestos,
            permiteCambios, permiteReembolso, vigenteHasta);

        await _repository.SaveAsync(fare);
        return fare;
    }
}
