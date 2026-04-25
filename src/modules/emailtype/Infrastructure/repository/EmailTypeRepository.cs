// src/modules/emailtype/Infrastructure/repository/EmailTypeRepository.cs
using Microsoft.EntityFrameworkCore;
using AirTicketSystem.shared.context;
using AirTicketSystem.modules.emailtype.Domain.aggregate;
using AirTicketSystem.modules.emailtype.Domain.Repositories;
using AirTicketSystem.modules.emailtype.Infrastructure.entity;

namespace AirTicketSystem.modules.emailtype.Infrastructure.repository;

public sealed class EmailTypeRepository : IEmailTypeRepository
{
    private readonly AppDbContext _context;

    public EmailTypeRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<EmailType?> FindByIdAsync(int id)
    {
        var entity = await _context.TiposEmail.FindAsync(id);
        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IReadOnlyCollection<EmailType>> FindAllAsync()
    {
        var entities = await _context.TiposEmail.OrderBy(e => e.Descripcion).ToListAsync();
        return entities.Select(MapToDomain).ToList().AsReadOnly();
    }

    public async Task<bool> ExistsByDescripcionAsync(string descripcion)
        => await _context.TiposEmail
            .AnyAsync(e => e.Descripcion.ToLower() == descripcion.ToLower());

    public async Task SaveAsync(EmailType emailType)
    {
        var entity = MapToEntity(emailType);
        await _context.TiposEmail.AddAsync(entity);
        await _context.SaveChangesAsync();
        emailType.EstablecerId(entity.Id);
    }

    public async Task UpdateAsync(EmailType emailType)
    {
        var entity = await _context.TiposEmail.FindAsync(emailType.Id)
            ?? throw new KeyNotFoundException(
                $"No se encontró el tipo de email con ID {emailType.Id} en la BD.");

        entity.Descripcion = emailType.Descripcion.Valor;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.TiposEmail.FindAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró el tipo de email con ID {id} en la BD.");

        _context.TiposEmail.Remove(entity);
        await _context.SaveChangesAsync();
    }

    private static EmailType MapToDomain(EmailTypeEntity entity)
        => EmailType.Reconstituir(entity.Id, entity.Descripcion);

    private static EmailTypeEntity MapToEntity(EmailType emailType)
        => new() { Descripcion = emailType.Descripcion.Valor };
}
