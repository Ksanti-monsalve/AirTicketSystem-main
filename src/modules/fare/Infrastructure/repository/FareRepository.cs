// src/modules/fare/Infrastructure/repository/FareRepository.cs
using Microsoft.EntityFrameworkCore;
using AirTicketSystem.shared.context;
using AirTicketSystem.modules.fare.Domain.aggregate;
using AirTicketSystem.modules.fare.Domain.Repositories;
using AirTicketSystem.modules.fare.Infrastructure.entity;

namespace AirTicketSystem.modules.fare.Infrastructure.repository;

public sealed class FareRepository : IFareRepository
{
    private readonly AppDbContext _context;

    public FareRepository(AppDbContext context) => _context = context;

    public async Task<Fare?> FindByIdAsync(int id)
    {
        var entity = await _context.Tarifas.FindAsync(id);
        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IReadOnlyCollection<Fare>> FindAllAsync()
    {
        var entities = await _context.Tarifas.ToListAsync();
        return entities.Select(MapToDomain).ToList();
    }

    public async Task<IReadOnlyCollection<Fare>> FindByRutaAsync(int rutaId)
    {
        var entities = await _context.Tarifas
            .Where(f => f.RutaId == rutaId)
            .OrderBy(f => f.PrecioTotal)
            .ToListAsync();
        return entities.Select(MapToDomain).ToList();
    }

    public async Task<IReadOnlyCollection<Fare>> FindByRutaAndClaseAsync(
        int rutaId, int claseServicioId)
    {
        var entities = await _context.Tarifas
            .Where(f =>
                f.RutaId == rutaId &&
                f.ClaseServicioId == claseServicioId &&
                f.Activa)
            .OrderBy(f => f.PrecioTotal)
            .ToListAsync();
        return entities.Select(MapToDomain).ToList();
    }

    public async Task<IReadOnlyCollection<Fare>> FindActivasAsync()
    {
        var entities = await _context.Tarifas
            .Where(f => f.Activa)
            .OrderBy(f => f.PrecioTotal)
            .ToListAsync();
        return entities.Select(MapToDomain).ToList();
    }

    public async Task<IReadOnlyCollection<Fare>> FindActivasByRutaAsync(int rutaId)
    {
        var hoy = DateOnly.FromDateTime(DateTime.Today);
        var entities = await _context.Tarifas
            .Where(f =>
                f.RutaId == rutaId &&
                f.Activa &&
                (f.VigenteHasta == null || f.VigenteHasta >= hoy))
            .OrderBy(f => f.PrecioTotal)
            .ToListAsync();
        return entities.Select(MapToDomain).ToList();
    }

    public async Task<bool> ExistsByRutaClaseNombreAsync(
        int rutaId, int claseServicioId, string nombre)
        => await _context.Tarifas
            .AnyAsync(f =>
                f.RutaId == rutaId &&
                f.ClaseServicioId == claseServicioId &&
                f.Nombre.ToLower() == nombre.ToLower());

    public async Task SaveAsync(Fare fare)
    {
        var entity = MapToEntity(fare);
        await _context.Tarifas.AddAsync(entity);
        await _context.SaveChangesAsync();
        fare.EstablecerId(entity.Id);
    }

    public async Task UpdateAsync(Fare fare)
    {
        var entity = await _context.Tarifas.FindAsync(fare.Id)
            ?? throw new KeyNotFoundException(
                $"Tarifa con ID {fare.Id} no encontrada.");
        entity.Nombre           = fare.Nombre.Valor;
        entity.PrecioBase       = fare.PrecioBase.Valor;
        entity.Impuestos        = fare.Impuestos.Valor;
        entity.PrecioTotal      = fare.PrecioTotal.Valor;
        entity.PermiteCambios   = fare.PermiteCambios.Valor;
        entity.PermiteReembolso = fare.PermiteReembolso.Valor;
        entity.Activa           = fare.Activa.Valor;
        entity.VigenteHasta     = fare.VigenteHasta?.Valor;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.Tarifas.FindAsync(id);
        if (entity is not null)
        {
            _context.Tarifas.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }

    private static Fare MapToDomain(FareEntity entity)
        => Fare.Reconstituir(
            entity.Id,
            entity.RutaId,
            entity.ClaseServicioId,
            entity.Nombre,
            entity.PrecioBase,
            entity.Impuestos,
            entity.PrecioTotal,
            entity.PermiteCambios,
            entity.PermiteReembolso,
            entity.Activa,
            entity.VigenteHasta);

    private static FareEntity MapToEntity(Fare fare)
        => new()
        {
            RutaId           = fare.RutaId,
            ClaseServicioId  = fare.ClaseServicioId,
            Nombre           = fare.Nombre.Valor,
            PrecioBase       = fare.PrecioBase.Valor,
            Impuestos        = fare.Impuestos.Valor,
            PrecioTotal      = fare.PrecioTotal.Valor,
            PermiteCambios   = fare.PermiteCambios.Valor,
            PermiteReembolso = fare.PermiteReembolso.Valor,
            Activa           = fare.Activa.Valor,
            VigenteHasta     = fare.VigenteHasta?.Valor
        };
}
