// src/modules/seatavailability/Infrastructure/repository/SeatAvailabilityRepository.cs
using Microsoft.EntityFrameworkCore;
using AirTicketSystem.shared.context;
using AirTicketSystem.modules.seatavailability.Domain.Repositories;
using AirTicketSystem.modules.seatavailability.Domain.aggregate;
using AirTicketSystem.modules.seatavailability.Infrastructure.entity;

namespace AirTicketSystem.modules.seatavailability.Infrastructure.repository;

public sealed class SeatAvailabilityRepository : ISeatAvailabilityRepository
{
    private readonly AppDbContext _context;

    public SeatAvailabilityRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<SeatAvailability?> FindByVueloAndAsientoAsync(
        int vueloId, int asientoId)
    {
        var entity = await _context.DisponibilidadAsientos
            .Include(sa => sa.Asiento)
                .ThenInclude(a => a.ClaseServicio)
            .FirstOrDefaultAsync(sa =>
                sa.VueloId == vueloId &&
                sa.AsientoId == asientoId);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IReadOnlyCollection<SeatAvailability>> FindByVueloAsync(int vueloId)
    {
        var entities = await _context.DisponibilidadAsientos
            .Include(sa => sa.Asiento)
                .ThenInclude(a => a.ClaseServicio)
            .Where(sa => sa.VueloId == vueloId)
            .OrderBy(sa => sa.Asiento.Fila)
            .ThenBy(sa => sa.Asiento.Columna)
            .ToListAsync();

        return entities.Select(MapToDomain).ToList().AsReadOnly();
    }

    public async Task<IReadOnlyCollection<SeatAvailability>> FindDisponiblesByVueloAsync(
        int vueloId)
    {
        var entities = await _context.DisponibilidadAsientos
            .Include(sa => sa.Asiento)
                .ThenInclude(a => a.ClaseServicio)
            .Where(sa => sa.VueloId == vueloId && sa.Estado == "DISPONIBLE")
            .OrderBy(sa => sa.Asiento.Fila)
            .ThenBy(sa => sa.Asiento.Columna)
            .ToListAsync();

        return entities.Select(MapToDomain).ToList().AsReadOnly();
    }

    public async Task<IReadOnlyCollection<SeatAvailability>>
        FindDisponiblesByVueloAndClaseAsync(int vueloId, int claseServicioId)
    {
        var entities = await _context.DisponibilidadAsientos
            .Include(sa => sa.Asiento)
            .Where(sa =>
                sa.VueloId == vueloId &&
                sa.Estado == "DISPONIBLE" &&
                sa.Asiento.ClaseServicioId == claseServicioId)
            .OrderBy(sa => sa.Asiento.Fila)
            .ThenBy(sa => sa.Asiento.Columna)
            .ToListAsync();

        return entities.Select(MapToDomain).ToList().AsReadOnly();
    }

    public async Task<int> ContarDisponiblesByVueloAsync(int vueloId)
        => await _context.DisponibilidadAsientos
            .CountAsync(sa =>
                sa.VueloId == vueloId &&
                sa.Estado == "DISPONIBLE");

    public async Task<bool> AsientoDisponibleAsync(int vueloId, int asientoId)
        => await _context.DisponibilidadAsientos
            .AnyAsync(sa =>
                sa.VueloId == vueloId &&
                sa.AsientoId == asientoId &&
                sa.Estado == "DISPONIBLE");

    public async Task SaveAllAsync(IEnumerable<SeatAvailability> asientos)
    {
        var entities = asientos.Select(MapToEntity).ToList();
        await _context.DisponibilidadAsientos.AddRangeAsync(entities);
        await _context.SaveChangesAsync();

        // Propagar los Ids generados
        var lista = asientos.ToList();
        for (int i = 0; i < lista.Count; i++)
            lista[i].EstablecerId(entities[i].Id);
    }

    public async Task UpdateAsync(SeatAvailability seatAvailability)
    {
        var entity = await _context.DisponibilidadAsientos
            .FindAsync(seatAvailability.Id)
            ?? throw new KeyNotFoundException(
                $"No se encontró la disponibilidad con ID {seatAvailability.Id}.");

        entity.Estado = seatAvailability.Estado.Valor;
        await _context.SaveChangesAsync();
    }

    private static SeatAvailability MapToDomain(SeatAvailabilityEntity entity)
        => SeatAvailability.Reconstituir(
            entity.Id,
            entity.VueloId,
            entity.AsientoId,
            entity.Estado);

    private static SeatAvailabilityEntity MapToEntity(SeatAvailability sa)
        => new SeatAvailabilityEntity
        {
            VueloId   = sa.VueloId,
            AsientoId = sa.AsientoId,
            Estado    = sa.Estado.Valor
        };
}