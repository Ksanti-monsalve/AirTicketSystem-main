// src/modules/person/Infrastructure/repository/PersonPhoneRepository.cs
using Microsoft.EntityFrameworkCore;
using AirTicketSystem.shared.context;
using AirTicketSystem.modules.person.Domain.aggregate;
using AirTicketSystem.modules.person.Domain.Repositories;
using AirTicketSystem.modules.person.Infrastructure.entity;

namespace AirTicketSystem.modules.person.Infrastructure.repository;

public sealed class PersonPhoneRepository : IPersonPhoneRepository
{
    private readonly AppDbContext _context;

    public PersonPhoneRepository(AppDbContext context) => _context = context;

    public async Task<PersonPhone?> FindByIdAsync(int id)
    {
        var entity = await _context.TelefonosPersona.FindAsync(id);
        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IReadOnlyCollection<PersonPhone>> FindByPersonaAsync(int personaId)
    {
        var entities = await _context.TelefonosPersona
            .Where(p => p.PersonaId == personaId)
            .OrderByDescending(p => p.EsPrincipal)
            .ToListAsync();
        return entities.Select(MapToDomain).ToList();
    }

    public async Task<PersonPhone?> FindPrincipalByPersonaAsync(int personaId)
    {
        var entity = await _context.TelefonosPersona
            .FirstOrDefaultAsync(p => p.PersonaId == personaId && p.EsPrincipal);
        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<bool> ExistsByNumeroAndPersonaAsync(string numero, int personaId)
        => await _context.TelefonosPersona
            .AnyAsync(p => p.Numero == numero && p.PersonaId == personaId);

    public async Task SaveAsync(PersonPhone phone)
    {
        var entity = MapToEntity(phone);
        _context.TelefonosPersona.Add(entity);
        await _context.SaveChangesAsync();
        phone.EstablecerId(entity.Id);
    }

    public async Task UpdateAsync(PersonPhone phone)
    {
        var entity = await _context.TelefonosPersona.FindAsync(phone.Id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un teléfono con ID {phone.Id}.");

        entity.Numero         = phone.Numero.Valor;
        entity.IndicativoPais = phone.IndicativoPais?.Valor;
        entity.EsPrincipal    = phone.EsPrincipal.Valor;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.TelefonosPersona.FindAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un teléfono con ID {id}.");

        _context.TelefonosPersona.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DesmarcarPrincipalByPersonaAsync(int personaId)
    {
        var telefonos = await _context.TelefonosPersona
            .Where(p => p.PersonaId == personaId && p.EsPrincipal)
            .ToListAsync();

        foreach (var telefono in telefonos)
            telefono.EsPrincipal = false;

        await _context.SaveChangesAsync();
    }

    private static PersonPhone MapToDomain(PersonPhoneEntity entity)
        => PersonPhone.Reconstituir(
            entity.Id,
            entity.PersonaId,
            entity.TipoTelefonoId,
            entity.Numero,
            entity.IndicativoPais,
            entity.EsPrincipal);

    private static PersonPhoneEntity MapToEntity(PersonPhone phone)
        => new()
        {
            PersonaId      = phone.PersonaId,
            TipoTelefonoId = phone.TipoTelefonoId,
            Numero         = phone.Numero.Valor,
            IndicativoPais = phone.IndicativoPais?.Valor,
            EsPrincipal    = phone.EsPrincipal.Valor
        };
}
