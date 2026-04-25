// src/modules/luggage/Infrastructure/repository/LuggageRepository.cs
using Microsoft.EntityFrameworkCore;
using AirTicketSystem.shared.context;
using AirTicketSystem.modules.luggage.Domain.aggregate;
using AirTicketSystem.modules.luggage.Domain.Repositories;
using AirTicketSystem.modules.luggage.Infrastructure.entity;

namespace AirTicketSystem.modules.luggage.Infrastructure.repository;

public sealed class LuggageRepository : ILuggageRepository
{
    private readonly AppDbContext _context;

    public LuggageRepository(AppDbContext context) => _context = context;

    public async Task<Luggage?> FindByIdAsync(int id)
    {
        var entity = await _context.Equipaje.FindAsync(id);
        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IReadOnlyCollection<Luggage>> FindByPasajeroReservaAsync(
        int pasajeroReservaId)
    {
        var entities = await _context.Equipaje
            .Where(l => l.PasajeroReservaId == pasajeroReservaId)
            .ToListAsync();
        return entities.Select(MapToDomain).ToList();
    }

    public async Task<IReadOnlyCollection<Luggage>> FindByVueloAsync(int vueloId)
    {
        var entities = await _context.Equipaje
            .Where(l => l.VueloId == vueloId)
            .OrderBy(l => l.CodigoEquipaje)
            .ToListAsync();
        return entities.Select(MapToDomain).ToList();
    }

    public async Task<Luggage?> FindByCodigoEquipajeAsync(string codigoEquipaje)
    {
        var entity = await _context.Equipaje
            .FirstOrDefaultAsync(l =>
                l.CodigoEquipaje == codigoEquipaje.ToUpperInvariant());
        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IReadOnlyCollection<Luggage>> FindByEstadoAsync(string estado)
    {
        var entities = await _context.Equipaje
            .Where(l => l.Estado == estado.ToUpperInvariant())
            .ToListAsync();
        return entities.Select(MapToDomain).ToList();
    }

    public async Task<IReadOnlyCollection<Luggage>> FindConIncidenciasAsync()
    {
        var entities = await _context.Equipaje
            .Where(l => l.Estado == "PERDIDO" || l.Estado == "DAÑADO")
            .OrderByDescending(l => l.VueloId)
            .ToListAsync();
        return entities.Select(MapToDomain).ToList();
    }

    public async Task<bool> ExistsByCodigoEquipajeAsync(string codigoEquipaje)
        => await _context.Equipaje
            .AnyAsync(l => l.CodigoEquipaje == codigoEquipaje.ToUpperInvariant());

    public async Task SaveAsync(Luggage luggage)
    {
        var entity = MapToEntity(luggage);
        _context.Equipaje.Add(entity);
        await _context.SaveChangesAsync();
        luggage.EstablecerId(entity.Id);
    }

    public async Task UpdateAsync(Luggage luggage)
    {
        var entity = await _context.Equipaje.FindAsync(luggage.Id)
            ?? throw new KeyNotFoundException(
                $"No se encontró el equipaje con ID {luggage.Id}.");

        entity.Estado          = luggage.Estado.Valor;
        entity.CodigoEquipaje  = luggage.CodigoEquipaje?.Valor;
        entity.CostoAdicional  = luggage.CostoAdicional.Valor;
        entity.PesoRealKg      = luggage.PesoRealKg?.Valor;
        entity.LargoRealCm     = luggage.LargoRealCm?.Valor;
        entity.AnchoRealCm     = luggage.AnchoRealCm?.Valor;
        entity.AltoRealCm      = luggage.AltoRealCm?.Valor;

        await _context.SaveChangesAsync();
    }

    private static Luggage MapToDomain(LuggageEntity e) =>
        Luggage.Reconstituir(
            e.Id,
            e.PasajeroReservaId,
            e.VueloId,
            e.TipoEquipajeId,
            e.Descripcion,
            e.PesoDeclaradoKg,
            e.LargoDeclaradoCm,
            e.AnchoDeclaradoCm,
            e.AltoDeclaradoCm,
            e.PesoRealKg,
            e.LargoRealCm,
            e.AnchoRealCm,
            e.AltoRealCm,
            e.CodigoEquipaje,
            e.CostoAdicional,
            e.Estado);

    private static LuggageEntity MapToEntity(Luggage l) => new()
    {
        PasajeroReservaId = l.PasajeroReservaId,
        VueloId           = l.VueloId,
        TipoEquipajeId    = l.TipoEquipajeId,
        Descripcion       = l.Descripcion?.Valor,
        PesoDeclaradoKg   = l.PesoDeclaradoKg?.Valor,
        LargoDeclaradoCm  = l.LargoDeclaradoCm?.Valor,
        AnchoDeclaradoCm  = l.AnchoDeclaradoCm?.Valor,
        AltoDeclaradoCm   = l.AltoDeclaradoCm?.Valor,
        PesoRealKg        = l.PesoRealKg?.Valor,
        LargoRealCm       = l.LargoRealCm?.Valor,
        AnchoRealCm       = l.AnchoRealCm?.Valor,
        AltoRealCm        = l.AltoRealCm?.Valor,
        CodigoEquipaje    = l.CodigoEquipaje?.Valor,
        CostoAdicional    = l.CostoAdicional.Valor,
        Estado            = l.Estado.Valor
    };
}
