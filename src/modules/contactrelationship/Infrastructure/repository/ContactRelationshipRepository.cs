// src/modules/contactrelationship/Infrastructure/repository/ContactRelationshipRepository.cs
using Microsoft.EntityFrameworkCore;
using AirTicketSystem.shared.context;
using AirTicketSystem.modules.contactrelationship.Domain.aggregate;
using AirTicketSystem.modules.contactrelationship.Domain.Repositories;
using AirTicketSystem.modules.contactrelationship.Infrastructure.entity;

namespace AirTicketSystem.modules.contactrelationship.Infrastructure.repository;

public sealed class ContactRelationshipRepository : IContactRelationshipRepository
{
    private readonly AppDbContext _context;

    public ContactRelationshipRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ContactRelationship?> FindByIdAsync(int id)
    {
        var entity = await _context.RelacionesContacto.FindAsync(id);
        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IReadOnlyCollection<ContactRelationship>> FindAllAsync()
    {
        var entities = await _context.RelacionesContacto.OrderBy(c => c.Descripcion).ToListAsync();
        return entities.Select(MapToDomain).ToList().AsReadOnly();
    }

    public async Task<bool> ExistsByDescripcionAsync(string descripcion)
        => await _context.RelacionesContacto
            .AnyAsync(c => c.Descripcion.ToLower() == descripcion.ToLower());

    public async Task SaveAsync(ContactRelationship contactRelationship)
    {
        var entity = MapToEntity(contactRelationship);
        await _context.RelacionesContacto.AddAsync(entity);
        await _context.SaveChangesAsync();
        contactRelationship.EstablecerId(entity.Id);
    }

    public async Task UpdateAsync(ContactRelationship contactRelationship)
    {
        var entity = await _context.RelacionesContacto.FindAsync(contactRelationship.Id)
            ?? throw new KeyNotFoundException(
                $"No se encontró la relación de contacto con ID {contactRelationship.Id} en la BD.");

        entity.Descripcion = contactRelationship.Descripcion.Valor;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.RelacionesContacto.FindAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró la relación de contacto con ID {id} en la BD.");

        _context.RelacionesContacto.Remove(entity);
        await _context.SaveChangesAsync();
    }

    private static ContactRelationship MapToDomain(ContactRelationshipEntity entity)
        => ContactRelationship.Reconstituir(entity.Id, entity.Descripcion);

    private static ContactRelationshipEntity MapToEntity(ContactRelationship contactRelationship)
        => new() { Descripcion = contactRelationship.Descripcion.Valor };
}
