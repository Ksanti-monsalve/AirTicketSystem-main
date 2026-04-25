// src/modules/route/Infrastructure/repository/RouteRepository.cs
using Microsoft.EntityFrameworkCore;
using AirTicketSystem.shared.context;
using AirTicketSystem.modules.route.Domain.aggregate;
using AirTicketSystem.modules.route.Domain.Repositories;
using AirTicketSystem.modules.route.Infrastructure.entity;

namespace AirTicketSystem.modules.route.Infrastructure.repository;

public sealed class RouteRepository : IRouteRepository
{
    private readonly AppDbContext _context;

    public RouteRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Route?> FindByIdAsync(int id)
    {
        var entity = await _context.Rutas
            .FirstOrDefaultAsync(r => r.Id == id);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IReadOnlyCollection<Route>> FindAllAsync()
    {
        var entities = await _context.Rutas
            .OrderBy(r => r.Id)
            .ToListAsync();

        return entities.Select(MapToDomain).ToList().AsReadOnly();
    }

    public async Task<IReadOnlyCollection<Route>> FindByAerolineaAsync(int aerolineaId)
    {
        var entities = await _context.Rutas
            .Include(r => r.Origen)
            .Include(r => r.Destino)
            .Where(r => r.AerolineaId == aerolineaId)
            .OrderBy(r => r.Origen.Nombre)
            .ToListAsync();
        return entities.Select(MapToDomain).ToList().AsReadOnly();
    }

    public async Task<IReadOnlyCollection<Route>> FindByOrigenAsync(int origenId)
    {
        var entities = await _context.Rutas
            .Include(r => r.Aerolinea)
            .Include(r => r.Destino)
            .Where(r => r.OrigenId == origenId && r.Activa)
            .OrderBy(r => r.Destino.Nombre)
            .ToListAsync();
        return entities.Select(MapToDomain).ToList().AsReadOnly();
    }

    public async Task<IReadOnlyCollection<Route>> FindByDestinoAsync(int destinoId)
    {
        var entities = await _context.Rutas
            .Include(r => r.Aerolinea)
            .Include(r => r.Origen)
            .Where(r => r.DestinoId == destinoId && r.Activa)
            .OrderBy(r => r.Origen.Nombre)
            .ToListAsync();
        return entities.Select(MapToDomain).ToList().AsReadOnly();
    }

    public async Task<IReadOnlyCollection<Route>> FindByOrigenAndDestinoAsync(
        int origenId, int destinoId)
    {
        var entities = await _context.Rutas
            .Include(r => r.Aerolinea)
            .Where(r =>
                r.OrigenId == origenId &&
                r.DestinoId == destinoId &&
                r.Activa)
            .ToListAsync();
        return entities.Select(MapToDomain).ToList().AsReadOnly();
    }

    public async Task<Route?> FindByAerolineaOrigenDestinoAsync(
        int aerolineaId, int origenId, int destinoId)
    {
        var entity = await _context.Rutas
            .Include(r => r.Aerolinea)
            .Include(r => r.Origen)
            .Include(r => r.Destino)
            .FirstOrDefaultAsync(r =>
                r.AerolineaId == aerolineaId &&
                r.OrigenId == origenId &&
                r.DestinoId == destinoId);
        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IReadOnlyCollection<Route>> FindActivasAsync()
    {
        var entities = await _context.Rutas
            .Include(r => r.Aerolinea)
            .Include(r => r.Origen)
            .Include(r => r.Destino)
            .Where(r => r.Activa)
            .OrderBy(r => r.Aerolinea.Nombre)
            .ToListAsync();
        return entities.Select(MapToDomain).ToList().AsReadOnly();
    }

    public async Task<bool> ExistsByAerolineaOrigenDestinoAsync(
        int aerolineaId, int origenId, int destinoId)
        => await _context.Rutas
            .AnyAsync(r =>
                r.AerolineaId == aerolineaId &&
                r.OrigenId == origenId &&
                r.DestinoId == destinoId);

    public async Task SaveAsync(Route route)
    {
        var entity = MapToEntity(route);
        await _context.Rutas.AddAsync(entity);
        await _context.SaveChangesAsync();
        route.EstablecerId(entity.Id);
    }

    public async Task UpdateAsync(Route route)
    {
        var entity = await _context.Rutas.FindAsync(route.Id)
            ?? throw new KeyNotFoundException(
                $"No se encontro la ruta con ID {route.Id} en la BD.");

        entity.DistanciaKm = route.DistanciaKm?.Valor;
        entity.DuracionEstimadaMin = route.DuracionEstimadaMin?.Valor;
        entity.Activa = route.Activa.Valor;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.Rutas.FindAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontro la ruta con ID {id} en la BD.");

        _context.Rutas.Remove(entity);
        await _context.SaveChangesAsync();
    }

    private static Route MapToDomain(RouteEntity entity)
        => Route.Reconstituir(
            entity.Id,
            entity.AerolineaId,
            entity.OrigenId,
            entity.DestinoId,
            entity.DistanciaKm,
            entity.DuracionEstimadaMin,
            entity.Activa);

    private static RouteEntity MapToEntity(Route route)
        => new()
        {
            AerolineaId = route.AerolineaId,
            OrigenId = route.OrigenId,
            DestinoId = route.DestinoId,
            DistanciaKm = route.DistanciaKm?.Valor,
            DuracionEstimadaMin = route.DuracionEstimadaMin?.Valor,
            Activa = route.Activa.Valor
        };
}