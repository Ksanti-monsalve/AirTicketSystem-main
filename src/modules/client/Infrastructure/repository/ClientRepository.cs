// src/modules/client/Infrastructure/repository/ClientRepository.cs
using Microsoft.EntityFrameworkCore;
using AirTicketSystem.shared.context;
using AirTicketSystem.modules.client.Domain.aggregate;
using AirTicketSystem.modules.client.Domain.Repositories;
using AirTicketSystem.modules.client.Infrastructure.entity;

namespace AirTicketSystem.modules.client.Infrastructure.repository;

public sealed class ClientRepository : IClientRepository
{
    private readonly AppDbContext _context;

    public ClientRepository(AppDbContext context) => _context = context;

    public async Task<Client?> FindByIdAsync(int id)
    {
        var entity = await _context.Clientes.FindAsync(id);
        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IReadOnlyCollection<Client>> FindAllAsync()
    {
        var entities = await _context.Clientes.ToListAsync();
        return entities.Select(MapToDomain).ToList();
    }

    public async Task<Client?> FindByPersonaAsync(int personaId)
    {
        var entity = await _context.Clientes
            .FirstOrDefaultAsync(c => c.PersonaId == personaId);
        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<Client?> FindByUsuarioAsync(int usuarioId)
    {
        var entity = await _context.Clientes
            .FirstOrDefaultAsync(c => c.UsuarioId == usuarioId);
        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IReadOnlyCollection<Client>> FindActivosAsync()
    {
        var entities = await _context.Clientes
            .Where(c => c.Activo)
            .ToListAsync();
        return entities.Select(MapToDomain).ToList();
    }

    public async Task SaveAsync(Client client)
    {
        var entity = MapToEntity(client);
        _context.Clientes.Add(entity);
        await _context.SaveChangesAsync();
        client.EstablecerId(entity.Id);
    }

    public async Task UpdateAsync(Client client)
    {
        var entity = await _context.Clientes.FindAsync(client.Id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un cliente con ID {client.Id}.");

        entity.Activo = client.Activo.Valor;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.Clientes.FindAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un cliente con ID {id}.");

        _context.Clientes.Remove(entity);
        await _context.SaveChangesAsync();
    }

    private static Client MapToDomain(ClientEntity entity)
        => Client.Reconstituir(
            entity.Id,
            entity.PersonaId,
            entity.UsuarioId,
            entity.Activo,
            entity.FechaRegistro);

    private static ClientEntity MapToEntity(Client client)
        => new()
        {
            PersonaId     = client.PersonaId,
            UsuarioId     = client.UsuarioId,
            Activo        = client.Activo.Valor,
            FechaRegistro = client.FechaRegistro.Valor
        };
}
