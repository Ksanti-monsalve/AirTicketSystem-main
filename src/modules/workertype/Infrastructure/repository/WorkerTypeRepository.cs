// src/modules/workertype/Infrastructure/repository/WorkerTypeRepository.cs
using Microsoft.EntityFrameworkCore;
using AirTicketSystem.shared.context;
using AirTicketSystem.modules.workertype.Domain.aggregate;
using AirTicketSystem.modules.workertype.Domain.Repositories;
using AirTicketSystem.modules.workertype.Infrastructure.entity;

namespace AirTicketSystem.modules.workertype.Infrastructure.repository;

public sealed class WorkerTypeRepository : IWorkerTypeRepository
{
    private readonly AppDbContext _context;

    public WorkerTypeRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<WorkerType?> FindByIdAsync(int id)
    {
        var entity = await _context.TiposTrabajador.FirstOrDefaultAsync(w => w.Id == id);
        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IReadOnlyCollection<WorkerType>> FindAllAsync()
    {
        var entities = await _context.TiposTrabajador.OrderBy(w => w.Nombre).ToListAsync();
        return entities.Select(MapToDomain).ToList().AsReadOnly();
    }

    public async Task<WorkerType?> FindByNombreAsync(string nombre)
    {
        var entity = await _context.TiposTrabajador
            .FirstOrDefaultAsync(w =>
                w.Nombre.ToLower() == nombre.ToLower());
        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<bool> ExistsByNombreAsync(string nombre)
        => await _context.TiposTrabajador
            .AnyAsync(w => w.Nombre.ToLower() == nombre.ToLower());

    public async Task SaveAsync(WorkerType workerType)
    {
        var entity = MapToEntity(workerType);
        await _context.TiposTrabajador.AddAsync(entity);
        await _context.SaveChangesAsync();
        workerType.EstablecerId(entity.Id);
    }

    public async Task UpdateAsync(WorkerType workerType)
    {
        var entity = await _context.TiposTrabajador.FindAsync(workerType.Id)
            ?? throw new KeyNotFoundException(
                $"No se encontro el tipo de trabajador con ID {workerType.Id} en la BD.");
        entity.Nombre = workerType.Nombre.Valor;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.TiposTrabajador.FindAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontro el tipo de trabajador con ID {id} en la BD.");
        _context.TiposTrabajador.Remove(entity);
        await _context.SaveChangesAsync();
    }

    private static WorkerType MapToDomain(WorkerTypeEntity entity)
        => WorkerType.Reconstituir(entity.Id, entity.Nombre);

    private static WorkerTypeEntity MapToEntity(WorkerType workerType)
        => new() { Nombre = workerType.Nombre.Valor };
}