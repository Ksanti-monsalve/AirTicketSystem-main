// src/modules/role/Infrastructure/repository/RoleRepository.cs
using Microsoft.EntityFrameworkCore;
using AirTicketSystem.shared.context;
using AirTicketSystem.modules.role.Domain.aggregate;
using AirTicketSystem.modules.role.Domain.Repositories;
using AirTicketSystem.modules.role.Infrastructure.entity;

namespace AirTicketSystem.modules.role.Infrastructure.repository;

public sealed class RoleRepository : IRoleRepository
{
    private readonly AppDbContext _context;

    public RoleRepository(AppDbContext context) => _context = context;

    public async Task<Role?> FindByIdAsync(int id)
    {
        var entity = await _context.Roles.FindAsync(id);
        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IReadOnlyCollection<Role>> FindAllAsync()
    {
        var entities = await _context.Roles.ToListAsync();
        return entities.Select(MapToDomain).ToList();
    }

    public async Task<bool> ExistsByNombreAsync(string nombre)
        => await _context.Roles
            .AnyAsync(r => r.Nombre == nombre.ToUpperInvariant());

    public async Task SaveAsync(Role role)
    {
        var entity = MapToEntity(role);
        await _context.Roles.AddAsync(entity);
        await _context.SaveChangesAsync();
        role.EstablecerId(entity.Id);
    }

    public async Task UpdateAsync(Role role)
    {
        var entity = await _context.Roles.FindAsync(role.Id)
            ?? throw new KeyNotFoundException(
                $"Rol con ID {role.Id} no encontrado.");
        entity.Nombre = role.Nombre.Valor;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.Roles.FindAsync(id);
        if (entity is not null)
        {
            _context.Roles.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }

    private static Role MapToDomain(RoleEntity entity)
        => Role.Reconstituir(entity.Id, entity.Nombre);

    private static RoleEntity MapToEntity(Role role)
        => new() { Nombre = role.Nombre.Valor };
}
