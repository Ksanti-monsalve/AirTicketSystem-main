// src/modules/worker/Infrastructure/repository/WorkerRepository.cs
using Microsoft.EntityFrameworkCore;
using AirTicketSystem.shared.context;
using AirTicketSystem.modules.worker.Domain.aggregate;
using AirTicketSystem.modules.worker.Domain.Repositories;
using AirTicketSystem.modules.worker.Infrastructure.entity;

namespace AirTicketSystem.modules.worker.Infrastructure.repository;

public sealed class WorkerRepository : IWorkerRepository
{
    private readonly AppDbContext _context;

    public WorkerRepository(AppDbContext context) => _context = context;

    public async Task<Worker?> FindByIdAsync(int id)
    {
        var entity = await _context.Trabajadores.FindAsync(id);
        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IReadOnlyCollection<Worker>> FindAllAsync()
    {
        var entities = await _context.Trabajadores.ToListAsync();
        return entities.Select(MapToDomain).ToList();
    }

    public async Task<Worker?> FindByPersonaAsync(int personaId)
    {
        var entity = await _context.Trabajadores
            .FirstOrDefaultAsync(w => w.PersonaId == personaId);
        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<Worker?> FindByUsuarioAsync(int usuarioId)
    {
        var entity = await _context.Trabajadores
            .FirstOrDefaultAsync(w => w.UsuarioId == usuarioId);
        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IReadOnlyCollection<Worker>> FindByAerolineaAsync(int aerolineaId)
    {
        var entities = await _context.Trabajadores
            .Where(w => w.AerolineaId == aerolineaId && w.Activo)
            .ToListAsync();
        return entities.Select(MapToDomain).ToList();
    }

    public async Task<IReadOnlyCollection<Worker>> FindByAeropuertoBaseAsync(int aeropuertoId)
    {
        var entities = await _context.Trabajadores
            .Where(w => w.AeropuertoBaseId == aeropuertoId && w.Activo)
            .ToListAsync();
        return entities.Select(MapToDomain).ToList();
    }

    public async Task<IReadOnlyCollection<Worker>> FindByTipoTrabajadorAsync(
        int tipoTrabajadorId)
    {
        var entities = await _context.Trabajadores
            .Where(w => w.TipoTrabajadorId == tipoTrabajadorId && w.Activo)
            .ToListAsync();
        return entities.Select(MapToDomain).ToList();
    }

    public async Task<IReadOnlyCollection<Worker>> FindActivosAsync()
    {
        var entities = await _context.Trabajadores
            .Where(w => w.Activo)
            .ToListAsync();
        return entities.Select(MapToDomain).ToList();
    }

    public async Task<IReadOnlyCollection<Worker>> FindPilotosHabilitadosParaModeloAsync(
        int modeloAvionId)
    {
        var hoy = DateOnly.FromDateTime(DateTime.Today);
        var entities = await _context.Trabajadores
            .Include(w => w.Licencias)
                .ThenInclude(l => l.Habilitaciones)
            .Where(w =>
                w.Activo &&
                w.Licencias.Any(l =>
                    l.Activa &&
                    l.FechaVencimiento >= hoy &&
                    l.Habilitaciones.Any(h =>
                        h.ModeloAvionId == modeloAvionId &&
                        h.FechaVencimiento >= hoy)))
            .ToListAsync();
        return entities.Select(MapToDomain).ToList();
    }

    public async Task SaveAsync(Worker worker)
    {
        var entity = MapToEntity(worker);
        _context.Trabajadores.Add(entity);
        await _context.SaveChangesAsync();
        worker.EstablecerId(entity.Id);
    }

    public async Task UpdateAsync(Worker worker)
    {
        var entity = await _context.Trabajadores.FindAsync(worker.Id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un trabajador con ID {worker.Id}.");

        entity.TipoTrabajadorId  = worker.TipoTrabajadorId;
        entity.AerolineaId       = worker.AerolineaId;
        entity.AeropuertoBaseId  = worker.AeropuertoBaseId;
        entity.FechaContratacion = worker.FechaContratacion.Valor;
        entity.Salario           = worker.Salario.Valor;
        entity.Activo            = worker.Activo.Valor;
        entity.UsuarioId         = worker.UsuarioId;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.Trabajadores.FindAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un trabajador con ID {id}.");

        _context.Trabajadores.Remove(entity);
        await _context.SaveChangesAsync();
    }

    private static Worker MapToDomain(WorkerEntity entity)
        => Worker.Reconstituir(
            entity.Id,
            entity.PersonaId,
            entity.TipoTrabajadorId,
            entity.AeropuertoBaseId,
            entity.FechaContratacion,
            entity.Salario,
            entity.Activo,
            entity.AerolineaId,
            entity.UsuarioId);

    private static WorkerEntity MapToEntity(Worker worker)
        => new()
        {
            PersonaId         = worker.PersonaId,
            TipoTrabajadorId  = worker.TipoTrabajadorId,
            AerolineaId       = worker.AerolineaId,
            AeropuertoBaseId  = worker.AeropuertoBaseId,
            FechaContratacion = worker.FechaContratacion.Valor,
            Salario           = worker.Salario.Valor,
            Activo            = worker.Activo.Valor,
            UsuarioId         = worker.UsuarioId
        };
}
