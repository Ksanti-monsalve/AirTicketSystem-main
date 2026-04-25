// src/modules/pilotrating/Infrastructure/repository/PilotRatingRepository.cs
using Microsoft.EntityFrameworkCore;
using AirTicketSystem.shared.context;
using AirTicketSystem.modules.pilotrating.Domain.aggregate;
using AirTicketSystem.modules.pilotrating.Domain.Repositories;
using AirTicketSystem.modules.pilotrating.Infrastructure.entity;

namespace AirTicketSystem.modules.pilotrating.Infrastructure.repository;

public sealed class PilotRatingRepository : IPilotRatingRepository
{
    private readonly AppDbContext _context;

    public PilotRatingRepository(AppDbContext context) => _context = context;

    public async Task<PilotRating?> FindByIdAsync(int id)
    {
        var entity = await _context.HabilitacionesPiloto.FindAsync(id);
        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IReadOnlyCollection<PilotRating>> FindAllAsync()
    {
        var entities = await _context.HabilitacionesPiloto.ToListAsync();
        return entities.Select(MapToDomain).ToList();
    }

    public async Task<IReadOnlyCollection<PilotRating>> FindByLicenciaAsync(int licenciaId)
    {
        var entities = await _context.HabilitacionesPiloto
            .Where(h => h.LicenciaId == licenciaId)
            .ToListAsync();
        return entities.Select(MapToDomain).ToList();
    }

    public async Task<IReadOnlyCollection<PilotRating>> FindByModeloAvionAsync(int modeloAvionId)
    {
        var entities = await _context.HabilitacionesPiloto
            .Where(h => h.ModeloAvionId == modeloAvionId)
            .ToListAsync();
        return entities.Select(MapToDomain).ToList();
    }

    public async Task<PilotRating?> FindByLicenciaAndModeloAsync(int licenciaId, int modeloAvionId)
    {
        var entity = await _context.HabilitacionesPiloto
            .FirstOrDefaultAsync(h =>
                h.LicenciaId == licenciaId && h.ModeloAvionId == modeloAvionId);
        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<bool> ExistsByLicenciaAndModeloAsync(int licenciaId, int modeloAvionId)
        => await _context.HabilitacionesPiloto
            .AnyAsync(h =>
                h.LicenciaId == licenciaId && h.ModeloAvionId == modeloAvionId);

    public async Task<IReadOnlyCollection<PilotRating>> FindVigentesAsync()
    {
        var hoy = DateOnly.FromDateTime(DateTime.Today);
        var entities = await _context.HabilitacionesPiloto
            .Where(h => h.FechaVencimiento >= hoy)
            .ToListAsync();
        return entities.Select(MapToDomain).ToList();
    }

    public async Task SaveAsync(PilotRating rating)
    {
        var entity = MapToEntity(rating);
        await _context.HabilitacionesPiloto.AddAsync(entity);
        await _context.SaveChangesAsync();
        rating.EstablecerId(entity.Id);
    }

    public async Task UpdateAsync(PilotRating rating)
    {
        var entity = await _context.HabilitacionesPiloto.FindAsync(rating.Id)
            ?? throw new KeyNotFoundException(
                $"Habilitación con ID {rating.Id} no encontrada.");
        entity.FechaHabilitacion = rating.FechaHabilitacion.Valor;
        entity.FechaVencimiento  = rating.FechaVencimiento.Valor;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.HabilitacionesPiloto.FindAsync(id);
        if (entity is not null)
        {
            _context.HabilitacionesPiloto.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }

    private static PilotRating MapToDomain(PilotRatingEntity entity)
        => PilotRating.Reconstituir(
            entity.Id,
            entity.LicenciaId,
            entity.ModeloAvionId,
            entity.FechaHabilitacion,
            entity.FechaVencimiento);

    private static PilotRatingEntity MapToEntity(PilotRating rating)
        => new()
        {
            LicenciaId        = rating.LicenciaId,
            ModeloAvionId     = rating.ModeloAvionId,
            FechaHabilitacion = rating.FechaHabilitacion.Valor,
            FechaVencimiento  = rating.FechaVencimiento.Valor
        };
}
