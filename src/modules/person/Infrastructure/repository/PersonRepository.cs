// src/modules/person/Infrastructure/repository/PersonRepository.cs
using Microsoft.EntityFrameworkCore;
using AirTicketSystem.shared.context;
using AirTicketSystem.modules.person.Domain.aggregate;
using AirTicketSystem.modules.person.Domain.Repositories;
using AirTicketSystem.modules.person.Infrastructure.entity;

namespace AirTicketSystem.modules.person.Infrastructure.repository;

public sealed class PersonRepository : IPersonRepository
{
    private readonly AppDbContext _context;

    public PersonRepository(AppDbContext context) => _context = context;

    public async Task<Person?> FindByIdAsync(int id)
    {
        var entity = await _context.Personas.FindAsync(id);
        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IReadOnlyCollection<Person>> FindAllAsync()
    {
        var entities = await _context.Personas
            .OrderBy(p => p.Apellidos)
            .ThenBy(p => p.Nombres)
            .ToListAsync();
        return entities.Select(MapToDomain).ToList();
    }

    public async Task<Person?> FindByDocumentoAsync(int tipoDocId, string numeroDoc)
    {
        var entity = await _context.Personas
            .FirstOrDefaultAsync(p =>
                p.TipoDocId == tipoDocId &&
                p.NumeroDoc == numeroDoc.ToUpperInvariant());
        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<bool> ExistsByDocumentoAsync(int tipoDocId, string numeroDoc)
        => await _context.Personas
            .AnyAsync(p =>
                p.TipoDocId == tipoDocId &&
                p.NumeroDoc == numeroDoc.ToUpperInvariant());

    public async Task SaveAsync(Person person)
    {
        var entity = MapToEntity(person);
        _context.Personas.Add(entity);
        await _context.SaveChangesAsync();
        person.EstablecerId(entity.Id);
    }

    public async Task UpdateAsync(Person person)
    {
        var entity = await _context.Personas.FindAsync(person.Id)
            ?? throw new KeyNotFoundException(
                $"No se encontró una persona con ID {person.Id}.");

        entity.TipoDocId       = person.TipoDocId;
        entity.NumeroDoc       = person.NumeroDoc.Valor;
        entity.Nombres         = person.Nombres.Valor;
        entity.Apellidos       = person.Apellidos.Valor;
        entity.FechaNacimiento = person.FechaNacimiento?.Valor;
        entity.GeneroId        = person.GeneroId;
        entity.NacionalidadId  = person.NacionalidadId;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.Personas.FindAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró una persona con ID {id}.");

        _context.Personas.Remove(entity);
        await _context.SaveChangesAsync();
    }

    private static Person MapToDomain(PersonEntity entity)
        => Person.Reconstituir(
            entity.Id,
            entity.TipoDocId,
            entity.NumeroDoc,
            entity.Nombres,
            entity.Apellidos,
            entity.FechaNacimiento,
            entity.GeneroId,
            entity.NacionalidadId);

    private static PersonEntity MapToEntity(Person person)
        => new()
        {
            TipoDocId       = person.TipoDocId,
            NumeroDoc       = person.NumeroDoc.Valor,
            Nombres         = person.Nombres.Valor,
            Apellidos       = person.Apellidos.Valor,
            FechaNacimiento = person.FechaNacimiento?.Valor,
            GeneroId        = person.GeneroId,
            NacionalidadId  = person.NacionalidadId
        };
}
