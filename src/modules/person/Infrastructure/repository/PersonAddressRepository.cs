// src/modules/person/Infrastructure/repository/PersonAddressRepository.cs
using Microsoft.EntityFrameworkCore;
using AirTicketSystem.shared.context;
using AirTicketSystem.modules.person.Domain.aggregate;
using AirTicketSystem.modules.person.Domain.Repositories;
using AirTicketSystem.modules.person.Infrastructure.entity;

namespace AirTicketSystem.modules.person.Infrastructure.repository;

public sealed class PersonAddressRepository : IPersonAddressRepository
{
    private readonly AppDbContext _context;

    public PersonAddressRepository(AppDbContext context) => _context = context;

    public async Task<PersonAddress?> FindByIdAsync(int id)
    {
        var entity = await _context.DireccionesPersona.FindAsync(id);
        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IReadOnlyCollection<PersonAddress>> FindByPersonaAsync(int personaId)
    {
        var entities = await _context.DireccionesPersona
            .Where(a => a.PersonaId == personaId)
            .OrderByDescending(a => a.EsPrincipal)
            .ToListAsync();
        return entities.Select(MapToDomain).ToList();
    }

    public async Task<PersonAddress?> FindPrincipalByPersonaAsync(int personaId)
    {
        var entity = await _context.DireccionesPersona
            .FirstOrDefaultAsync(a => a.PersonaId == personaId && a.EsPrincipal);
        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IReadOnlyCollection<PersonAddress>> FindByPersonaAndTipoAsync(
        int personaId, int tipoDireccionId)
    {
        var entities = await _context.DireccionesPersona
            .Where(a => a.PersonaId == personaId && a.TipoDireccionId == tipoDireccionId)
            .ToListAsync();
        return entities.Select(MapToDomain).ToList();
    }

    public async Task SaveAsync(PersonAddress address)
    {
        var entity = MapToEntity(address);
        _context.DireccionesPersona.Add(entity);
        await _context.SaveChangesAsync();
        address.EstablecerId(entity.Id);
    }

    public async Task UpdateAsync(PersonAddress address)
    {
        var entity = await _context.DireccionesPersona.FindAsync(address.Id)
            ?? throw new KeyNotFoundException(
                $"No se encontró una dirección con ID {address.Id}.");

        entity.TipoDireccionId = address.TipoDireccionId;
        entity.CiudadId        = address.CiudadId;
        entity.DireccionLinea1 = address.DireccionLinea1.Valor;
        entity.DireccionLinea2 = address.DireccionLinea2?.Valor;
        entity.CodigoPostal    = address.CodigoPostal?.Valor;
        entity.EsPrincipal     = address.EsPrincipal.Valor;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.DireccionesPersona.FindAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró una dirección con ID {id}.");

        _context.DireccionesPersona.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DesmarcarPrincipalByPersonaAsync(int personaId)
    {
        var direcciones = await _context.DireccionesPersona
            .Where(a => a.PersonaId == personaId && a.EsPrincipal)
            .ToListAsync();

        foreach (var dir in direcciones)
            dir.EsPrincipal = false;

        await _context.SaveChangesAsync();
    }

    private static PersonAddress MapToDomain(PersonAddressEntity entity)
        => PersonAddress.Reconstituir(
            entity.Id,
            entity.PersonaId,
            entity.TipoDireccionId,
            entity.CiudadId,
            entity.DireccionLinea1,
            entity.DireccionLinea2,
            entity.CodigoPostal,
            entity.EsPrincipal);

    private static PersonAddressEntity MapToEntity(PersonAddress address)
        => new()
        {
            PersonaId       = address.PersonaId,
            TipoDireccionId = address.TipoDireccionId,
            CiudadId        = address.CiudadId,
            DireccionLinea1 = address.DireccionLinea1.Valor,
            DireccionLinea2 = address.DireccionLinea2?.Valor,
            CodigoPostal    = address.CodigoPostal?.Valor,
            EsPrincipal     = address.EsPrincipal.Valor
        };
}
