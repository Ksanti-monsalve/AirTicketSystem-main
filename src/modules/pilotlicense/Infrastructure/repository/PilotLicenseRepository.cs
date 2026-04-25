// src/modules/pilotlicense/Infrastructure/repository/PilotLicenseRepository.cs
using Microsoft.EntityFrameworkCore;
using AirTicketSystem.shared.context;
using AirTicketSystem.modules.pilotlicense.Domain.aggregate;
using AirTicketSystem.modules.pilotlicense.Domain.Repositories;
using AirTicketSystem.modules.pilotlicense.Infrastructure.entity;

namespace AirTicketSystem.modules.pilotlicense.Infrastructure.repository;

public sealed class PilotLicenseRepository : IPilotLicenseRepository
{
    private readonly AppDbContext _context;

    public PilotLicenseRepository(AppDbContext context) => _context = context;

    public async Task<PilotLicense?> FindByIdAsync(int id)
    {
        var entity = await _context.LicenciasPiloto.FindAsync(id);
        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IReadOnlyCollection<PilotLicense>> FindAllAsync()
    {
        var entities = await _context.LicenciasPiloto.ToListAsync();
        return entities.Select(MapToDomain).ToList();
    }

    public async Task<IReadOnlyCollection<PilotLicense>> FindByTrabajadorAsync(int trabajadorId)
    {
        var entities = await _context.LicenciasPiloto
            .Where(l => l.TrabajadorId == trabajadorId)
            .ToListAsync();
        return entities.Select(MapToDomain).ToList();
    }

    public async Task<IReadOnlyCollection<PilotLicense>> FindVigentesAsync()
    {
        var hoy = DateOnly.FromDateTime(DateTime.Today);
        var entities = await _context.LicenciasPiloto
            .Where(l => l.Activa && l.FechaVencimiento >= hoy)
            .ToListAsync();
        return entities.Select(MapToDomain).ToList();
    }

    public async Task<IReadOnlyCollection<PilotLicense>> FindProximasAVencerAsync(int diasUmbral)
    {
        var hoy    = DateOnly.FromDateTime(DateTime.Today);
        var umbral = hoy.AddDays(diasUmbral);
        var entities = await _context.LicenciasPiloto
            .Where(l => l.Activa && l.FechaVencimiento >= hoy && l.FechaVencimiento <= umbral)
            .ToListAsync();
        return entities.Select(MapToDomain).ToList();
    }

    public async Task<bool> ExistsByNumeroLicenciaAsync(string numeroLicencia)
        => await _context.LicenciasPiloto
            .AnyAsync(l => l.NumeroLicencia == numeroLicencia);

    public async Task SaveAsync(PilotLicense license)
    {
        var entity = MapToEntity(license);
        await _context.LicenciasPiloto.AddAsync(entity);
        await _context.SaveChangesAsync();
        license.EstablecerId(entity.Id);
    }

    public async Task UpdateAsync(PilotLicense license)
    {
        var entity = await _context.LicenciasPiloto.FindAsync(license.Id)
            ?? throw new KeyNotFoundException(
                $"Licencia con ID {license.Id} no encontrada.");
        entity.NumeroLicencia   = license.NumeroLicencia.Valor;
        entity.TipoLicencia     = license.TipoLicencia.Valor;
        entity.FechaExpedicion  = license.FechaExpedicion.Valor;
        entity.FechaVencimiento = license.FechaVencimiento.Valor;
        entity.AutoridadEmisora = license.AutoridadEmisora.Valor;
        entity.Activa           = license.Activa.Valor;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.LicenciasPiloto.FindAsync(id);
        if (entity is not null)
        {
            _context.LicenciasPiloto.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }

    private static PilotLicense MapToDomain(PilotLicenseEntity entity)
        => PilotLicense.Reconstituir(
            entity.Id,
            entity.TrabajadorId,
            entity.NumeroLicencia,
            entity.TipoLicencia,
            entity.FechaExpedicion,
            entity.FechaVencimiento,
            entity.AutoridadEmisora,
            entity.Activa);

    private static PilotLicenseEntity MapToEntity(PilotLicense license)
        => new()
        {
            TrabajadorId     = license.TrabajadorId,
            NumeroLicencia   = license.NumeroLicencia.Valor,
            TipoLicencia     = license.TipoLicencia.Valor,
            FechaExpedicion  = license.FechaExpedicion.Valor,
            FechaVencimiento = license.FechaVencimiento.Valor,
            AutoridadEmisora = license.AutoridadEmisora.Valor,
            Activa           = license.Activa.Valor
        };
}
