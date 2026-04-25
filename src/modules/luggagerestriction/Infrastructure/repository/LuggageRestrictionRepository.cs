// src/modules/luggagerestriction/Infrastructure/repository/LuggageRestrictionRepository.cs
using Microsoft.EntityFrameworkCore;
using AirTicketSystem.shared.context;
using AirTicketSystem.modules.luggagerestriction.Domain.aggregate;
using AirTicketSystem.modules.luggagerestriction.Domain.Repositories;
using AirTicketSystem.modules.luggagerestriction.Infrastructure.entity;

namespace AirTicketSystem.modules.luggagerestriction.Infrastructure.repository;

public sealed class LuggageRestrictionRepository : ILuggageRestrictionRepository
{
    private readonly AppDbContext _context;

    public LuggageRestrictionRepository(AppDbContext context) => _context = context;

    public async Task<LuggageRestriction?> FindByIdAsync(int id)
    {
        var entity = await _context.RestriccionesEquipaje.FindAsync(id);
        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IReadOnlyCollection<LuggageRestriction>> FindAllAsync()
    {
        var entities = await _context.RestriccionesEquipaje.ToListAsync();
        return entities.Select(MapToDomain).ToList();
    }

    public async Task<IReadOnlyCollection<LuggageRestriction>> FindByTarifaAsync(int tarifaId)
    {
        var entities = await _context.RestriccionesEquipaje
            .Where(r => r.TarifaId == tarifaId)
            .ToListAsync();
        return entities.Select(MapToDomain).ToList();
    }

    public async Task<LuggageRestriction?> FindByTarifaAndTipoEquipajeAsync(
        int tarifaId, int tipoEquipajeId)
    {
        var entity = await _context.RestriccionesEquipaje
            .FirstOrDefaultAsync(r =>
                r.TarifaId == tarifaId && r.TipoEquipajeId == tipoEquipajeId);
        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<bool> ExistsByTarifaAndTipoEquipajeAsync(
        int tarifaId, int tipoEquipajeId)
        => await _context.RestriccionesEquipaje
            .AnyAsync(r =>
                r.TarifaId == tarifaId && r.TipoEquipajeId == tipoEquipajeId);

    public async Task SaveAsync(LuggageRestriction restriction)
    {
        var entity = MapToEntity(restriction);
        await _context.RestriccionesEquipaje.AddAsync(entity);
        await _context.SaveChangesAsync();
        restriction.EstablecerId(entity.Id);
    }

    public async Task UpdateAsync(LuggageRestriction restriction)
    {
        var entity = await _context.RestriccionesEquipaje.FindAsync(restriction.Id)
            ?? throw new KeyNotFoundException(
                $"Restricción con ID {restriction.Id} no encontrada.");
        entity.PiezasIncluidas = restriction.PiezasIncluidas.Valor;
        entity.PesoMaximoKg    = restriction.PesoMaximoKg.Valor;
        entity.CostoExcesoKg   = restriction.CostoExcesoKg.Valor;
        entity.LargoMaxCm      = restriction.LargoMaxCm?.Valor;
        entity.AnchoMaxCm      = restriction.AnchoMaxCm?.Valor;
        entity.AltoMaxCm       = restriction.AltoMaxCm?.Valor;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.RestriccionesEquipaje.FindAsync(id);
        if (entity is not null)
        {
            _context.RestriccionesEquipaje.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }

    private static LuggageRestriction MapToDomain(LuggageRestrictionEntity entity)
        => LuggageRestriction.Reconstituir(
            entity.Id,
            entity.TarifaId,
            entity.TipoEquipajeId,
            entity.PiezasIncluidas,
            entity.PesoMaximoKg,
            entity.CostoExcesoKg,
            entity.LargoMaxCm,
            entity.AnchoMaxCm,
            entity.AltoMaxCm);

    private static LuggageRestrictionEntity MapToEntity(LuggageRestriction restriction)
        => new()
        {
            TarifaId        = restriction.TarifaId,
            TipoEquipajeId  = restriction.TipoEquipajeId,
            PiezasIncluidas = restriction.PiezasIncluidas.Valor,
            PesoMaximoKg    = restriction.PesoMaximoKg.Valor,
            CostoExcesoKg   = restriction.CostoExcesoKg.Valor,
            LargoMaxCm      = restriction.LargoMaxCm?.Valor,
            AnchoMaxCm      = restriction.AnchoMaxCm?.Valor,
            AltoMaxCm       = restriction.AltoMaxCm?.Valor
        };
}
