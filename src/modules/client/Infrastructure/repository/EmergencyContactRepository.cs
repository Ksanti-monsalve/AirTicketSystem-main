// src/modules/client/Infrastructure/repository/EmergencyContactRepository.cs
using Microsoft.EntityFrameworkCore;
using AirTicketSystem.shared.context;
using AirTicketSystem.modules.client.Domain.aggregate;
using AirTicketSystem.modules.client.Domain.Repositories;
using AirTicketSystem.modules.client.Infrastructure.entity;

namespace AirTicketSystem.modules.client.Infrastructure.repository;

public sealed class EmergencyContactRepository : IEmergencyContactRepository
{
    private readonly AppDbContext _context;

    public EmergencyContactRepository(AppDbContext context) => _context = context;

    public async Task<EmergencyContact?> FindByIdAsync(int id)
    {
        var entity = await _context.ContactosEmergencia.FindAsync(id);
        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IReadOnlyCollection<EmergencyContact>> FindByClienteAsync(int clienteId)
    {
        var entities = await _context.ContactosEmergencia
            .Where(ce => ce.ClienteId == clienteId)
            .OrderByDescending(ce => ce.EsPrincipal)
            .ToListAsync();
        return entities.Select(MapToDomain).ToList();
    }

    public async Task<EmergencyContact?> FindPrincipalByClienteAsync(int clienteId)
    {
        var entity = await _context.ContactosEmergencia
            .FirstOrDefaultAsync(ce => ce.ClienteId == clienteId && ce.EsPrincipal);
        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<bool> ExistsByClienteAndPersonaAsync(int clienteId, int personaId)
        => await _context.ContactosEmergencia
            .AnyAsync(ce => ce.ClienteId == clienteId && ce.PersonaId == personaId);

    public async Task SaveAsync(EmergencyContact contact)
    {
        var entity = MapToEntity(contact);
        _context.ContactosEmergencia.Add(entity);
        await _context.SaveChangesAsync();
        contact.EstablecerId(entity.Id);
    }

    public async Task UpdateAsync(EmergencyContact contact)
    {
        var entity = await _context.ContactosEmergencia.FindAsync(contact.Id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un contacto de emergencia con ID {contact.Id}.");

        entity.RelacionId  = contact.RelacionId;
        entity.EsPrincipal = contact.EsPrincipal.Valor;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.ContactosEmergencia.FindAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un contacto de emergencia con ID {id}.");

        _context.ContactosEmergencia.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DesmarcarPrincipalByClienteAsync(int clienteId)
    {
        var contactos = await _context.ContactosEmergencia
            .Where(ce => ce.ClienteId == clienteId && ce.EsPrincipal)
            .ToListAsync();

        foreach (var contacto in contactos)
            contacto.EsPrincipal = false;

        await _context.SaveChangesAsync();
    }

    private static EmergencyContact MapToDomain(EmergencyContactEntity entity)
        => EmergencyContact.Reconstituir(
            entity.Id,
            entity.ClienteId,
            entity.PersonaId,
            entity.RelacionId,
            entity.EsPrincipal);

    private static EmergencyContactEntity MapToEntity(EmergencyContact contact)
        => new()
        {
            ClienteId   = contact.ClienteId,
            PersonaId   = contact.PersonaId,
            RelacionId  = contact.RelacionId,
            EsPrincipal = contact.EsPrincipal.Valor
        };
}
