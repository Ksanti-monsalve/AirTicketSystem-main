// src/shared/helpers/BaseRepository.cs
using Microsoft.EntityFrameworkCore;
using AirTicketSystem.shared.contracts;
using AirTicketSystem.shared.context;

namespace AirTicketSystem.shared.helpers;

/// <summary>
/// Implementación base de las operaciones CRUD comunes.
/// Todos los repositorios heredan de esta clase.
/// </summary>
public abstract class BaseRepository<T> : IRepository<T> where T : class
{
    protected readonly AppDbContext _context;
    protected readonly DbSet<T> _dbSet;

    protected BaseRepository(AppDbContext context)
    {
        _context = context;
        _dbSet   = context.Set<T>();
    }

    public virtual async Task<T?> GetByIdAsync(int id)
        => await _dbSet.FindAsync(id);

    public virtual async Task<IEnumerable<T>> GetAllAsync()
        => await _dbSet.ToListAsync();

    public virtual async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public virtual async Task UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
        await _context.SaveChangesAsync();
    }

    public virtual async Task DeleteAsync(int id)
    {
        var entity = await _dbSet.FindAsync(id);
        if (entity is null)
            throw new KeyNotFoundException(
                $"No se encontró el registro con Id {id}.");

        _dbSet.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public virtual async Task<bool> ExistsAsync(int id)
        => await _dbSet.FindAsync(id) is not null;
}