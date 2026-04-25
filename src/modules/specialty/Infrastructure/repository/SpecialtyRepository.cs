// src/modules/specialty/Infrastructure/repository/SpecialtyRepository.cs
using Microsoft.EntityFrameworkCore;
using AirTicketSystem.shared.context;
using AirTicketSystem.modules.specialty.Domain.aggregate;
using AirTicketSystem.modules.specialty.Domain.Repositories;
using AirTicketSystem.modules.specialty.Infrastructure.entity;

namespace AirTicketSystem.modules.specialty.Infrastructure.repository;

public sealed class SpecialtyRepository : ISpecialtyRepository
{
    private readonly AppDbContext _context;

    public SpecialtyRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Specialty?> FindByIdAsync(int id)
    {
        var entity = await _context.Especialidades.FirstOrDefaultAsync(s => s.Id == id);
        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IReadOnlyCollection<Specialty>> FindAllAsync()
    {
        var entities = await _context.Especialidades.OrderBy(s => s.Nombre).ToListAsync();
        return entities.Select(MapToDomain).ToList().AsReadOnly();
    }

    public async Task<IReadOnlyCollection<Specialty>> FindByTipoTrabajadorAsync(
        int tipoTrabajadorId)
    {
        var entities = await _context.Especialidades
            .Where(s => s.TipoTrabajadorId == tipoTrabajadorId)
            .OrderBy(s => s.Nombre)
            .ToListAsync();
        return entities.Select(MapToDomain).ToList().AsReadOnly();
    }

    public async Task<IReadOnlyCollection<Specialty>> FindGeneralesAsync()
    {
        var entities = await _context.Especialidades
            .Where(s => s.TipoTrabajadorId == null)
            .OrderBy(s => s.Nombre)
            .ToListAsync();
        return entities.Select(MapToDomain).ToList().AsReadOnly();
    }

    public async Task<bool> ExistsByNombreAsync(string nombre)
        => await _context.Especialidades
            .AnyAsync(s => s.Nombre.ToLower() == nombre.ToLower());

    public async Task SaveAsync(Specialty specialty)
    {
        var entity = MapToEntity(specialty);
        await _context.Especialidades.AddAsync(entity);
        await _context.SaveChangesAsync();
        specialty.EstablecerId(entity.Id);
    }

    public async Task UpdateAsync(Specialty specialty)
    {
        var entity = await _context.Especialidades.FindAsync(specialty.Id)
            ?? throw new KeyNotFoundException(
                $"No se encontro la especialidad con ID {specialty.Id} en la BD.");

        entity.Nombre = specialty.Nombre.Valor;
        entity.TipoTrabajadorId = specialty.TipoTrabajadorId;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.Especialidades.FindAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontro la especialidad con ID {id} en la BD.");
        _context.Especialidades.Remove(entity);
        await _context.SaveChangesAsync();
    }

    private static Specialty MapToDomain(SpecialtyEntity entity)
        => Specialty.Reconstituir(entity.Id, entity.Nombre, entity.TipoTrabajadorId);

    private static SpecialtyEntity MapToEntity(Specialty specialty)
        => new()
        {
            Nombre = specialty.Nombre.Valor,
            TipoTrabajadorId = specialty.TipoTrabajadorId
        };
}