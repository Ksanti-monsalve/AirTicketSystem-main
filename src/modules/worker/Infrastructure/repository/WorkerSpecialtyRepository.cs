// src/modules/worker/Infrastructure/repository/WorkerSpecialtyRepository.cs
using Microsoft.EntityFrameworkCore;
using AirTicketSystem.shared.context;
using AirTicketSystem.modules.worker.Domain.aggregate;
using AirTicketSystem.modules.worker.Domain.Repositories;
using AirTicketSystem.modules.worker.Infrastructure.entity;

namespace AirTicketSystem.modules.worker.Infrastructure.repository;

public sealed class WorkerSpecialtyRepository : IWorkerSpecialtyRepository
{
    private readonly AppDbContext _context;

    public WorkerSpecialtyRepository(AppDbContext context) => _context = context;

    public async Task<WorkerSpecialty?> FindByIdAsync(int id)
    {
        var entity = await _context.TrabajadorEspecialidades.FindAsync(id);
        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IReadOnlyCollection<WorkerSpecialty>> FindByTrabajadorAsync(
        int trabajadorId)
    {
        var entities = await _context.TrabajadorEspecialidades
            .Where(ws => ws.TrabajadorId == trabajadorId)
            .ToListAsync();
        return entities.Select(MapToDomain).ToList();
    }

    public async Task<IReadOnlyCollection<WorkerSpecialty>> FindByEspecialidadAsync(
        int especialidadId)
    {
        var entities = await _context.TrabajadorEspecialidades
            .Where(ws => ws.EspecialidadId == especialidadId)
            .ToListAsync();
        return entities.Select(MapToDomain).ToList();
    }

    public async Task<bool> ExistsByTrabajadorAndEspecialidadAsync(
        int trabajadorId, int especialidadId)
        => await _context.TrabajadorEspecialidades
            .AnyAsync(ws =>
                ws.TrabajadorId == trabajadorId &&
                ws.EspecialidadId == especialidadId);

    public async Task SaveAsync(WorkerSpecialty specialty)
    {
        var entity = MapToEntity(specialty);
        _context.TrabajadorEspecialidades.Add(entity);
        await _context.SaveChangesAsync();
        specialty.EstablecerId(entity.Id);
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.TrabajadorEspecialidades.FindAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró una especialidad de trabajador con ID {id}.");

        _context.TrabajadorEspecialidades.Remove(entity);
        await _context.SaveChangesAsync();
    }

    private static WorkerSpecialty MapToDomain(WorkerSpecialtyEntity entity)
        => WorkerSpecialty.Reconstituir(
            entity.Id,
            entity.TrabajadorId,
            entity.EspecialidadId);

    private static WorkerSpecialtyEntity MapToEntity(WorkerSpecialty specialty)
        => new()
        {
            TrabajadorId   = specialty.TrabajadorId,
            EspecialidadId = specialty.EspecialidadId
        };
}
