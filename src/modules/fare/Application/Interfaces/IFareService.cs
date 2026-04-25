// src/modules/fare/Application/Interfaces/IFareService.cs
using AirTicketSystem.modules.fare.Domain.aggregate;

namespace AirTicketSystem.modules.fare.Application.Interfaces;

public interface IFareService
{
    Task<Fare> CreateAsync(
        int rutaId, int claseServicioId, string nombre,
        decimal precioBase, decimal impuestos,
        bool permiteCambios, bool permiteReembolso,
        DateOnly? vigenteHasta);
    Task<Fare> GetByIdAsync(int id);
    Task<IReadOnlyCollection<Fare>> GetAllAsync();
    Task<IReadOnlyCollection<Fare>> GetByRouteAsync(int rutaId);
    Task<IReadOnlyCollection<Fare>> GetActivasAsync();
    Task<IReadOnlyCollection<Fare>> GetActivasByRouteAsync(int rutaId);
    Task<Fare> UpdateAsync(
        int id, string nombre,
        decimal precioBase, decimal impuestos,
        bool permiteCambios, bool permiteReembolso,
        DateOnly? vigenteHasta);
    Task ActivateAsync(int id);
    Task DeactivateAsync(int id);
    Task DeleteAsync(int id);
}
