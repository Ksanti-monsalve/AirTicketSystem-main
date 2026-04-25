// src/shared/contracts/IRepository.cs
namespace AirTicketSystem.shared.contracts;

/// <summary>
/// Contrato base para todos los repositorios del sistema.
/// Define las operaciones CRUD comunes a cualquier entidad.
/// </summary>
public interface IRepository<T> where T : class
{
    Task<T?> GetByIdAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
}