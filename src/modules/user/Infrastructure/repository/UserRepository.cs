// src/modules/user/Infrastructure/repository/UserRepository.cs
using Microsoft.EntityFrameworkCore;
using AirTicketSystem.shared.context;
using AirTicketSystem.modules.user.Domain.aggregate;
using AirTicketSystem.modules.user.Domain.Repositories;
using AirTicketSystem.modules.user.Infrastructure.entity;

namespace AirTicketSystem.modules.user.Infrastructure.repository;

public sealed class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context) => _context = context;

    public async Task<User?> FindByIdAsync(int id)
    {
        var entity = await _context.Usuarios.FindAsync(id);
        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IReadOnlyCollection<User>> FindAllAsync()
    {
        var entities = await _context.Usuarios
            .OrderBy(u => u.Username)
            .ToListAsync();
        return entities.Select(MapToDomain).ToList();
    }

    public async Task<User?> FindByUsernameAsync(string username)
    {
        var entity = await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Username == username.ToLowerInvariant());
        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<User?> FindByPersonaAsync(int personaId)
    {
        var entity = await _context.Usuarios
            .FirstOrDefaultAsync(u => u.PersonaId == personaId);
        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IReadOnlyCollection<User>> FindByRolAsync(int rolId)
    {
        var entities = await _context.Usuarios
            .Where(u => u.RolId == rolId)
            .OrderBy(u => u.Username)
            .ToListAsync();
        return entities.Select(MapToDomain).ToList();
    }

    public async Task<IReadOnlyCollection<User>> FindActivosAsync()
    {
        var entities = await _context.Usuarios
            .Where(u => u.Activo)
            .OrderBy(u => u.Username)
            .ToListAsync();
        return entities.Select(MapToDomain).ToList();
    }

    public async Task<bool> ExistsByUsernameAsync(string username)
        => await _context.Usuarios
            .AnyAsync(u => u.Username == username.ToLowerInvariant());

    public async Task SaveAsync(User user)
    {
        var entity = MapToEntity(user);
        _context.Usuarios.Add(entity);
        await _context.SaveChangesAsync();
        user.EstablecerId(entity.Id);
    }

    public async Task UpdateAsync(User user)
    {
        var entity = await _context.Usuarios.FindAsync(user.Id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un usuario con ID {user.Id}.");

        entity.RolId            = user.RolId;
        entity.Username         = user.Username.Valor;
        entity.PasswordHash     = user.PasswordHash.Valor;
        entity.Activo           = user.Activo.Valor;
        entity.UltimoLogin      = user.UltimoLogin?.Valor;
        entity.IntentosFallidos = user.IntentosFallidos.Valor;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.Usuarios.FindAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un usuario con ID {id}.");

        _context.Usuarios.Remove(entity);
        await _context.SaveChangesAsync();
    }

    private static User MapToDomain(UserEntity entity)
        => User.Reconstituir(
            entity.Id,
            entity.PersonaId,
            entity.RolId,
            entity.Username,
            entity.PasswordHash,
            entity.Activo,
            entity.FechaRegistro,
            entity.UltimoLogin,
            entity.IntentosFallidos);

    private static UserEntity MapToEntity(User user)
        => new()
        {
            PersonaId        = user.PersonaId,
            RolId            = user.RolId,
            Username         = user.Username.Valor,
            PasswordHash     = user.PasswordHash.Valor,
            Activo           = user.Activo.Valor,
            FechaRegistro    = user.FechaRegistro.Valor,
            UltimoLogin      = user.UltimoLogin?.Valor,
            IntentosFallidos = user.IntentosFallidos.Valor
        };
}
