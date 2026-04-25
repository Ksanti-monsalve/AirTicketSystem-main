// src/modules/person/Infrastructure/repository/PersonEmailRepository.cs
using Microsoft.EntityFrameworkCore;
using AirTicketSystem.shared.context;
using AirTicketSystem.modules.person.Domain.aggregate;
using AirTicketSystem.modules.person.Domain.Repositories;
using AirTicketSystem.modules.person.Infrastructure.entity;

namespace AirTicketSystem.modules.person.Infrastructure.repository;

public sealed class PersonEmailRepository : IPersonEmailRepository
{
    private readonly AppDbContext _context;

    public PersonEmailRepository(AppDbContext context) => _context = context;

    public async Task<PersonEmail?> FindByIdAsync(int id)
    {
        var entity = await _context.EmailsPersona.FindAsync(id);
        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IReadOnlyCollection<PersonEmail>> FindByPersonaAsync(int personaId)
    {
        var entities = await _context.EmailsPersona
            .Where(e => e.PersonaId == personaId)
            .OrderByDescending(e => e.EsPrincipal)
            .ToListAsync();
        return entities.Select(MapToDomain).ToList();
    }

    public async Task<PersonEmail?> FindPrincipalByPersonaAsync(int personaId)
    {
        var entity = await _context.EmailsPersona
            .FirstOrDefaultAsync(e => e.PersonaId == personaId && e.EsPrincipal);
        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<PersonEmail?> FindByEmailAsync(string email)
    {
        var entity = await _context.EmailsPersona
            .FirstOrDefaultAsync(e => e.Email.ToLower() == email.ToLower());
        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<bool> ExistsByEmailAsync(string email)
        => await _context.EmailsPersona
            .AnyAsync(e => e.Email.ToLower() == email.ToLower());

    public async Task SaveAsync(PersonEmail email)
    {
        var entity = MapToEntity(email);
        _context.EmailsPersona.Add(entity);
        await _context.SaveChangesAsync();
        email.EstablecerId(entity.Id);
    }

    public async Task UpdateAsync(PersonEmail email)
    {
        var entity = await _context.EmailsPersona.FindAsync(email.Id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un email con ID {email.Id}.");

        entity.Email       = email.Email.Valor;
        entity.EsPrincipal = email.EsPrincipal.Valor;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.EmailsPersona.FindAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un email con ID {id}.");

        _context.EmailsPersona.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DesmarcarPrincipalByPersonaAsync(int personaId)
    {
        var emails = await _context.EmailsPersona
            .Where(e => e.PersonaId == personaId && e.EsPrincipal)
            .ToListAsync();

        foreach (var e in emails)
            e.EsPrincipal = false;

        await _context.SaveChangesAsync();
    }

    private static PersonEmail MapToDomain(PersonEmailEntity entity)
        => PersonEmail.Reconstituir(
            entity.Id,
            entity.PersonaId,
            entity.TipoEmailId,
            entity.Email,
            entity.EsPrincipal);

    private static PersonEmailEntity MapToEntity(PersonEmail email)
        => new()
        {
            PersonaId   = email.PersonaId,
            TipoEmailId = email.TipoEmailId,
            Email       = email.Email.Valor,
            EsPrincipal = email.EsPrincipal.Valor
        };
}
