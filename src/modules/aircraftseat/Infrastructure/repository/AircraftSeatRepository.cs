// src/modules/aircraftseat/Infrastructure/repository/AircraftSeatRepository.cs
using Microsoft.EntityFrameworkCore;
using AirTicketSystem.shared.context;
using AirTicketSystem.modules.aircraftseat.Domain.aggregate;
using AirTicketSystem.modules.aircraftseat.Domain.Repositories;
using AirTicketSystem.modules.aircraftseat.Infrastructure.entity;

namespace AirTicketSystem.modules.aircraftseat.Infrastructure.repository;

public sealed class AircraftSeatRepository : IAircraftSeatRepository
{
    private readonly AppDbContext _context;

    public AircraftSeatRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<AircraftSeat?> FindByIdAsync(int id)
    {
        var entity = await _context.AsientosAvion.FirstOrDefaultAsync(a => a.Id == id);
        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<AircraftSeat?> FindByCodigoAndAvionAsync(
        string codigoAsiento, int avionId)
    {
        var entity = await _context.AsientosAvion
            .Include(a => a.ClaseServicio)
            .FirstOrDefaultAsync(a =>
                a.CodigoAsiento == codigoAsiento.ToUpperInvariant() &&
                a.AvionId == avionId);
        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IReadOnlyCollection<AircraftSeat>> FindByAvionAsync(int avionId)
    {
        var entities = await _context.AsientosAvion
            .Include(a => a.ClaseServicio)
            .Where(a => a.AvionId == avionId && a.Activo)
            .OrderBy(a => a.Fila)
            .ThenBy(a => a.Columna)
            .ToListAsync();
        return entities.Select(MapToDomain).ToList().AsReadOnly();
    }

    public async Task<IReadOnlyCollection<AircraftSeat>> FindByAvionAndClaseAsync(
        int avionId, int claseServicioId)
    {
        var entities = await _context.AsientosAvion
            .Include(a => a.ClaseServicio)
            .Where(a =>
                a.AvionId == avionId &&
                a.ClaseServicioId == claseServicioId &&
                a.Activo)
            .OrderBy(a => a.Fila)
            .ThenBy(a => a.Columna)
            .ToListAsync();
        return entities.Select(MapToDomain).ToList().AsReadOnly();
    }

    public async Task<int> ContarAsientosByAvionAsync(int avionId)
        => await _context.AsientosAvion
            .CountAsync(a => a.AvionId == avionId && a.Activo);

    public async Task<bool> ExistsByCodigoAndAvionAsync(
        string codigoAsiento, int avionId)
        => await _context.AsientosAvion
            .AnyAsync(a =>
                a.CodigoAsiento == codigoAsiento.ToUpperInvariant() &&
                a.AvionId == avionId);

    public async Task SaveAsync(AircraftSeat seat)
    {
        var entity = MapToEntity(seat);
        await _context.AsientosAvion.AddAsync(entity);
        await _context.SaveChangesAsync();
        seat.EstablecerId(entity.Id);
    }

    public async Task UpdateAsync(AircraftSeat seat)
    {
        var entity = await _context.AsientosAvion.FindAsync(seat.Id)
            ?? throw new KeyNotFoundException($"No se encontro el asiento con ID {seat.Id} en la BD.");

        entity.EsVentana = seat.EsVentana.Valor;
        entity.EsPasillo = seat.EsPasillo.Valor;
        entity.Activo = seat.Activo.Valor;
        entity.CostoSeleccion = seat.CostoSeleccion.Valor;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.AsientosAvion.FindAsync(id)
            ?? throw new KeyNotFoundException($"No se encontro el asiento con ID {id} en la BD.");
        _context.AsientosAvion.Remove(entity);
        await _context.SaveChangesAsync();
    }

    private static AircraftSeat MapToDomain(AircraftSeatEntity entity)
        => AircraftSeat.Reconstituir(
            entity.Id,
            entity.AvionId,
            entity.ClaseServicioId,
            entity.CodigoAsiento,
            entity.Fila,
            entity.Columna,
            entity.EsVentana,
            entity.EsPasillo,
            entity.Activo,
            entity.CostoSeleccion);

    private static AircraftSeatEntity MapToEntity(AircraftSeat seat)
        => new()
        {
            AvionId = seat.AvionId,
            ClaseServicioId = seat.ClaseServicioId,
            CodigoAsiento = seat.CodigoAsiento.Valor,
            Fila = seat.Fila.Valor,
            Columna = seat.Columna.Valor.ToString(),
            EsVentana = seat.EsVentana.Valor,
            EsPasillo = seat.EsPasillo.Valor,
            Activo = seat.Activo.Valor,
            CostoSeleccion = seat.CostoSeleccion.Valor
        };
}