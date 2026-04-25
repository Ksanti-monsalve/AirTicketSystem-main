// src/modules/fare/Domain/Repositories/IFareRepository.cs
using AirTicketSystem.modules.fare.Domain.aggregate;

namespace AirTicketSystem.modules.fare.Domain.Repositories;

public interface IFareRepository
{
    Task<Fare?> FindByIdAsync(int id);
    Task<IReadOnlyCollection<Fare>> FindAllAsync();
    Task<IReadOnlyCollection<Fare>> FindByRutaAsync(int rutaId);
    Task<IReadOnlyCollection<Fare>> FindByRutaAndClaseAsync(int rutaId, int claseServicioId);
    Task<IReadOnlyCollection<Fare>> FindActivasAsync();
    Task<IReadOnlyCollection<Fare>> FindActivasByRutaAsync(int rutaId);
    Task<bool> ExistsByRutaClaseNombreAsync(int rutaId, int claseServicioId, string nombre);
    Task SaveAsync(Fare fare);
    Task UpdateAsync(Fare fare);
    Task DeleteAsync(int id);
}
