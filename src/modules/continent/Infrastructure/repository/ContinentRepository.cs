// src/modules/continent/Infrastructure/repository/ContinentRepository.cs
using Microsoft.EntityFrameworkCore;
using AirTicketSystem.shared.context;
using AirTicketSystem.modules.continent.Domain.aggregate;
using AirTicketSystem.modules.continent.Domain.Repositories;
using AirTicketSystem.modules.continent.Infrastructure.entity;

namespace AirTicketSystem.modules.continent.Infrastructure.repository;

public sealed class ContinentRepository : IContinentRepository
{
    private readonly AppDbContext _context;

    public ContinentRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Continent?> FindByIdAsync(int id)
    {
        var entity = await _context.Continentes.FindAsync(id);
        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IReadOnlyCollection<Continent>> FindAllAsync()
    {
        var entities = await _context.Continentes.OrderBy(c => c.Nombre).ToListAsync();
        return entities.Select(MapToDomain).ToList().AsReadOnly();
    }

    public async Task<Continent?> FindByCodigoAsync(string codigo)
    {
        var entity = await _context.Continentes
            .FirstOrDefaultAsync(c => c.Codigo == codigo.ToUpperInvariant());
        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<bool> ExistsByCodigoAsync(string codigo)
        => await _context.Continentes
            .AnyAsync(c => c.Codigo == codigo.ToUpperInvariant());

    public async Task<bool> ExistsByNombreAsync(string nombre)
        => await _context.Continentes
            .AnyAsync(c => c.Nombre.ToLower() == nombre.ToLower());

    public async Task SaveAsync(Continent continent)
    {
        var entity = MapToEntity(continent);
        await _context.Continentes.AddAsync(entity);
        await _context.SaveChangesAsync();
        continent.EstablecerId(entity.Id);
    }

    public async Task UpdateAsync(Continent continent)
    {
        var entity = await _context.Continentes.FindAsync(continent.Id)
            ?? throw new KeyNotFoundException(
                $"No se encontró el continente con ID {continent.Id} en la BD.");

        entity.Nombre = continent.Nombre.Valor;
        entity.Codigo = continent.Codigo.Valor;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.Continentes.FindAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró el continente con ID {id} en la BD.");

        _context.Continentes.Remove(entity);
        await _context.SaveChangesAsync();
    }

    private static Continent MapToDomain(ContinentEntity entity)
        => Continent.Reconstituir(entity.Id, entity.Nombre, entity.Codigo);

    private static ContinentEntity MapToEntity(Continent continent)
        => new()
        {
            Nombre = continent.Nombre.Valor,
            Codigo = continent.Codigo.Valor
        };
}
