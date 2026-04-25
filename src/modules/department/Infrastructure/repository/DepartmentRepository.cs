// src/modules/department/Infrastructure/repository/DepartmentRepository.cs
using Microsoft.EntityFrameworkCore;
using AirTicketSystem.shared.context;
using AirTicketSystem.modules.department.Domain.aggregate;
using AirTicketSystem.modules.department.Domain.Repositories;
using AirTicketSystem.modules.department.Infrastructure.entity;

namespace AirTicketSystem.modules.department.Infrastructure.repository;

public sealed class DepartmentRepository : IDepartmentRepository
{
    private readonly AppDbContext _context;

    public DepartmentRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Department?> FindByIdAsync(int id)
    {
        var entity = await _context.Departamentos.FindAsync(id);
        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IReadOnlyCollection<Department>> FindAllAsync()
    {
        var entities = await _context.Departamentos
            .OrderBy(d => d.Nombre)
            .ToListAsync();
        return entities.Select(MapToDomain).ToList();
    }

    public async Task<IReadOnlyCollection<Department>> FindByRegionAsync(int regionId)
    {
        var entities = await _context.Departamentos
            .Where(d => d.RegionId == regionId)
            .OrderBy(d => d.Nombre)
            .ToListAsync();
        return entities.Select(MapToDomain).ToList();
    }

    public async Task<bool> ExistsByNombreAndRegionAsync(string nombre, int regionId)
        => await _context.Departamentos
            .AnyAsync(d =>
                d.Nombre.ToLower() == nombre.ToLower() &&
                d.RegionId == regionId);

    public async Task SaveAsync(Department department)
    {
        var entity = MapToEntity(department);
        await _context.Departamentos.AddAsync(entity);
        await _context.SaveChangesAsync();
        department.EstablecerId(entity.Id);
    }

    public async Task UpdateAsync(Department department)
    {
        var entity = await _context.Departamentos.FindAsync(department.Id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un departamento con ID {department.Id}.");

        entity.RegionId = department.RegionId;
        entity.Nombre   = department.Nombre.Valor;
        entity.Codigo   = department.Codigo?.Valor;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.Departamentos.FindAsync(id);
        if (entity is not null)
        {
            _context.Departamentos.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }

    private static Department MapToDomain(DepartmentEntity entity)
        => Department.Reconstituir(entity.Id, entity.RegionId, entity.Nombre, entity.Codigo);

    private static DepartmentEntity MapToEntity(Department department)
        => new()
        {
            RegionId = department.RegionId,
            Nombre   = department.Nombre.Valor,
            Codigo   = department.Codigo?.Valor
        };
}
