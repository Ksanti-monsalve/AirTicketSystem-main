// src/modules/fare/Application/Services/FareService.cs
using AirTicketSystem.modules.fare.Application.Interfaces;
using AirTicketSystem.modules.fare.Application.UseCases;
using AirTicketSystem.modules.fare.Domain.aggregate;

namespace AirTicketSystem.modules.fare.Application.Services;

public sealed class FareService : IFareService
{
    private readonly CreateFareUseCase _create;
    private readonly GetFareByIdUseCase _getById;
    private readonly GetAllFaresUseCase _getAll;
    private readonly GetFaresByRouteUseCase _getByRoute;
    private readonly GetActiveFaresUseCase _getActivas;
    private readonly GetActiveFaresByRouteUseCase _getActivasByRoute;
    private readonly UpdateFareUseCase _update;
    private readonly ActivateFareUseCase _activate;
    private readonly DeactivateFareUseCase _deactivate;
    private readonly DeleteFareUseCase _delete;

    public FareService(
        CreateFareUseCase create,
        GetFareByIdUseCase getById,
        GetAllFaresUseCase getAll,
        GetFaresByRouteUseCase getByRoute,
        GetActiveFaresUseCase getActivas,
        GetActiveFaresByRouteUseCase getActivasByRoute,
        UpdateFareUseCase update,
        ActivateFareUseCase activate,
        DeactivateFareUseCase deactivate,
        DeleteFareUseCase delete)
    {
        _create           = create;
        _getById          = getById;
        _getAll           = getAll;
        _getByRoute       = getByRoute;
        _getActivas       = getActivas;
        _getActivasByRoute = getActivasByRoute;
        _update           = update;
        _activate         = activate;
        _deactivate       = deactivate;
        _delete           = delete;
    }

    public Task<Fare> CreateAsync(
        int rutaId, int claseServicioId, string nombre,
        decimal precioBase, decimal impuestos,
        bool permiteCambios, bool permiteReembolso,
        DateOnly? vigenteHasta)
        => _create.ExecuteAsync(
            rutaId, claseServicioId, nombre,
            precioBase, impuestos,
            permiteCambios, permiteReembolso, vigenteHasta);

    public Task<Fare> GetByIdAsync(int id)
        => _getById.ExecuteAsync(id);

    public Task<IReadOnlyCollection<Fare>> GetAllAsync()
        => _getAll.ExecuteAsync();

    public Task<IReadOnlyCollection<Fare>> GetByRouteAsync(int rutaId)
        => _getByRoute.ExecuteAsync(rutaId);

    public Task<IReadOnlyCollection<Fare>> GetActivasAsync()
        => _getActivas.ExecuteAsync();

    public Task<IReadOnlyCollection<Fare>> GetActivasByRouteAsync(int rutaId)
        => _getActivasByRoute.ExecuteAsync(rutaId);

    public Task<Fare> UpdateAsync(
        int id, string nombre,
        decimal precioBase, decimal impuestos,
        bool permiteCambios, bool permiteReembolso,
        DateOnly? vigenteHasta)
        => _update.ExecuteAsync(
            id, nombre, precioBase, impuestos,
            permiteCambios, permiteReembolso, vigenteHasta);

    public Task ActivateAsync(int id)   => _activate.ExecuteAsync(id);
    public Task DeactivateAsync(int id) => _deactivate.ExecuteAsync(id);
    public Task DeleteAsync(int id)     => _delete.ExecuteAsync(id);
}
