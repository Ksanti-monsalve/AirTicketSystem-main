// src/modules/region/Infrastructure/repository/RegionRepository.cs
using Microsoft.EntityFrameworkCore;
using AirTicketSystem.shared.context;
using AirTicketSystem.modules.region.Domain.aggregate;
using AirTicketSystem.modules.region.Domain.Repositories;
using AirTicketSystem.modules.region.Infrastructure.entity;

namespace AirTicketSystem.modules.region.Infrastructure.repository;

public sealed class RegionRepository : IRegionRepository
{
    private readonly AppDbContext _context;

    public RegionRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Region?> FindByIdAsync(int id)
    {
        var entity = await _context.Regiones.FindAsync(id);
        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IReadOnlyCollection<Region>> FindAllAsync()
    {
        var entities = await _context.Regiones
            .OrderBy(r => r.Nombre)
            .ToListAsync();
        return entities.Select(MapToDomain).ToList();
    }

    public async Task<IReadOnlyCollection<Region>> FindByPaisAsync(int paisId)
    {
        var entities = await _context.Regiones
            .Where(r => r.PaisId == paisId)
            .OrderBy(r => r.Nombre)
            .ToListAsync();
        return entities.Select(MapToDomain).ToList();
    }

    public async Task<bool> ExistsByNombreAndPaisAsync(string nombre, int paisId)
        => await _context.Regiones
            .AnyAsync(r =>
                r.Nombre.ToLower() == nombre.ToLower() &&
                r.PaisId == paisId);

    public async Task SaveAsync(Region region)
    {
        var entity = MapToEntity(region);
        await _context.Regiones.AddAsync(entity);
        await _context.SaveChangesAsync();
        region.EstablecerId(entity.Id);
    }

    public async Task UpdateAsync(Region region)
    {
        var entity = await _context.Regiones.FindAsync(region.Id)
            ?? throw new KeyNotFoundException(
                $"No se encontró una región con ID {region.Id}.");

        entity.PaisId = region.PaisId;
        entity.Nombre = region.Nombre.Valor;
        entity.Codigo = region.Codigo?.Valor;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.Regiones.FindAsync(id);
        if (entity is not null)
        {
            _context.Regiones.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }

    private static Region MapToDomain(RegionEntity entity)
        => Region.Reconstituir(entity.Id, entity.PaisId, entity.Nombre, entity.Codigo);

    private static RegionEntity MapToEntity(Region region)
        => new()
        {
            PaisId = region.PaisId,
            Nombre = region.Nombre.Valor,
            Codigo = region.Codigo?.Valor
        };
}
