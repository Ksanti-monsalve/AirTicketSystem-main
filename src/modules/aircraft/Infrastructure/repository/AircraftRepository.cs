using Microsoft.EntityFrameworkCore;
using AirTicketSystem.shared.context;
using AirTicketSystem.modules.aircraft.Domain.aggregate;
using AirTicketSystem.modules.aircraft.Domain.Repositories;
using AirTicketSystem.modules.aircraft.Infrastructure.entity;

namespace AirTicketSystem.modules.aircraft.Infrastructure.repository;

public sealed class AircraftRepository : IAircraftRepository
{
    private readonly AppDbContext _context;

    public AircraftRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Aircraft?> FindByIdAsync(int id)
    {
        var entity = await _context.Aviones.FirstOrDefaultAsync(a => a.Id == id);
        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IReadOnlyCollection<Aircraft>> FindAllAsync()
    {
        var entities = await _context.Aviones.OrderBy(a => a.Matricula).ToListAsync();
        return entities.Select(MapToDomain).ToList().AsReadOnly();
    }

    public async Task<Aircraft?> FindByMatriculaAsync(string matricula)
    {
        var entity = await _context.Aviones.FirstOrDefaultAsync(
            a => a.Matricula == matricula.ToUpperInvariant());
        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IReadOnlyCollection<Aircraft>> FindByAerolineaAsync(int aerolineaId)
    {
        var entities = await _context.Aviones
            .Where(a => a.AerolineaId == aerolineaId)
            .OrderBy(a => a.Matricula)
            .ToListAsync();
        return entities.Select(MapToDomain).ToList().AsReadOnly();
    }

    public async Task<IReadOnlyCollection<Aircraft>> FindByModeloAsync(int modeloAvionId)
    {
        var entities = await _context.Aviones
            .Where(a => a.ModeloAvionId == modeloAvionId)
            .OrderBy(a => a.Matricula)
            .ToListAsync();
        return entities.Select(MapToDomain).ToList().AsReadOnly();
    }

    public async Task<IReadOnlyCollection<Aircraft>> FindByEstadoAsync(string estado)
    {
        var entities = await _context.Aviones
            .Where(a => a.Estado == estado)
            .OrderBy(a => a.Matricula)
            .ToListAsync();
        return entities.Select(MapToDomain).ToList().AsReadOnly();
    }

    public async Task<IReadOnlyCollection<Aircraft>> FindDisponiblesAsync()
    {
        var entities = await _context.Aviones
            .Where(a => a.Estado == "DISPONIBLE" && a.Activo)
            .OrderBy(a => a.Matricula)
            .ToListAsync();
        return entities.Select(MapToDomain).ToList().AsReadOnly();
    }

    public async Task<IReadOnlyCollection<Aircraft>> FindConMantenimientoUrgenteAsync()
    {
        var today = DateOnly.FromDateTime(DateTime.Today);
        var entities = await _context.Aviones
            .Where(a => a.FechaProximoMantenimiento != null && a.FechaProximoMantenimiento <= today && a.Activo)
            .OrderBy(a => a.FechaProximoMantenimiento)
            .ToListAsync();
        return entities.Select(MapToDomain).ToList().AsReadOnly();
    }

    public Task<bool> ExistsByMatriculaAsync(string matricula)
        => _context.Aviones.AnyAsync(a => a.Matricula == matricula.ToUpperInvariant());

    public async Task SaveAsync(Aircraft aircraft)
    {
        var entity = MapToEntity(aircraft);
        await _context.Aviones.AddAsync(entity);
        await _context.SaveChangesAsync();
        aircraft.EstablecerId(entity.Id);
    }

    public async Task UpdateAsync(Aircraft aircraft)
    {
        var entity = await _context.Aviones.FindAsync(aircraft.Id)
            ?? throw new KeyNotFoundException($"No se encontro el avion con ID {aircraft.Id} en la BD.");

        entity.FechaFabricacion = aircraft.FechaFabricacion?.Valor;
        entity.FechaUltimoMantenimiento = aircraft.FechaUltimoMantenimiento?.Valor;
        entity.FechaProximoMantenimiento = aircraft.FechaProximoMantenimiento?.Valor;
        entity.TotalHorasVuelo = aircraft.TotalHorasVuelo.Valor;
        entity.Estado = aircraft.Estado.Valor;
        entity.Activo = aircraft.Activo.Valor;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.Aviones.FindAsync(id)
            ?? throw new KeyNotFoundException($"No se encontro el avion con ID {id} en la BD.");
        _context.Aviones.Remove(entity);
        await _context.SaveChangesAsync();
    }

    private static Aircraft MapToDomain(AircraftEntity entity)
        => Aircraft.Reconstituir(
            entity.Id,
            entity.ModeloAvionId,
            entity.AerolineaId,
            entity.Matricula,
            entity.FechaFabricacion,
            entity.FechaUltimoMantenimiento,
            entity.FechaProximoMantenimiento,
            entity.TotalHorasVuelo,
            entity.Estado,
            entity.Activo);

    private static AircraftEntity MapToEntity(Aircraft aircraft)
        => new()
        {
            ModeloAvionId = aircraft.ModeloAvionId,
            AerolineaId = aircraft.AerolineaId,
            Matricula = aircraft.Matricula.Valor,
            FechaFabricacion = aircraft.FechaFabricacion?.Valor,
            FechaUltimoMantenimiento = aircraft.FechaUltimoMantenimiento?.Valor,
            FechaProximoMantenimiento = aircraft.FechaProximoMantenimiento?.Valor,
            TotalHorasVuelo = aircraft.TotalHorasVuelo.Valor,
            Estado = aircraft.Estado.Valor,
            Activo = aircraft.Activo.Valor
        };
}