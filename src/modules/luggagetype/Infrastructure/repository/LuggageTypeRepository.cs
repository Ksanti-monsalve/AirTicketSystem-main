// src/modules/luggagetype/Infrastructure/repository/LuggageTypeRepository.cs
using Microsoft.EntityFrameworkCore;
using AirTicketSystem.shared.context;
using AirTicketSystem.modules.luggagetype.Domain.aggregate;
using AirTicketSystem.modules.luggagetype.Domain.Repositories;
using AirTicketSystem.modules.luggagetype.Infrastructure.entity;

namespace AirTicketSystem.modules.luggagetype.Infrastructure.repository;

public sealed class LuggageTypeRepository : ILuggageTypeRepository
{
    private readonly AppDbContext _context;

    public LuggageTypeRepository(AppDbContext context) => _context = context;

    public async Task<LuggageType?> FindByIdAsync(int id)
    {
        var entity = await _context.TiposEquipaje.FindAsync(id);
        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IReadOnlyCollection<LuggageType>> FindAllAsync()
    {
        var entities = await _context.TiposEquipaje.ToListAsync();
        return entities.Select(MapToDomain).ToList();
    }

    public async Task<bool> ExistsByNombreAsync(string nombre)
        => await _context.TiposEquipaje
            .AnyAsync(l => l.Nombre.ToLower() == nombre.ToLower());

    public async Task SaveAsync(LuggageType luggageType)
    {
        var entity = MapToEntity(luggageType);
        await _context.TiposEquipaje.AddAsync(entity);
        await _context.SaveChangesAsync();
        luggageType.EstablecerId(entity.Id);
    }

    public async Task UpdateAsync(LuggageType luggageType)
    {
        var entity = await _context.TiposEquipaje.FindAsync(luggageType.Id)
            ?? throw new KeyNotFoundException(
                $"Tipo de equipaje con ID {luggageType.Id} no encontrado.");
        entity.Nombre = luggageType.Nombre.Valor;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.TiposEquipaje.FindAsync(id);
        if (entity is not null)
        {
            _context.TiposEquipaje.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }

    private static LuggageType MapToDomain(LuggageTypeEntity entity)
        => LuggageType.Reconstituir(entity.Id, entity.Nombre);

    private static LuggageTypeEntity MapToEntity(LuggageType luggageType)
        => new() { Nombre = luggageType.Nombre.Valor };
}
