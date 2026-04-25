// src/modules/country/Infrastructure/repository/CountryRepository.cs
using Microsoft.EntityFrameworkCore;
using AirTicketSystem.shared.context;
using AirTicketSystem.modules.country.Domain.aggregate;
using AirTicketSystem.modules.country.Domain.Repositories;
using AirTicketSystem.modules.country.Infrastructure.entity;

namespace AirTicketSystem.modules.country.Infrastructure.repository;

public sealed class CountryRepository : ICountryRepository
{
    private readonly AppDbContext _context;

    public CountryRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Country?> FindByIdAsync(int id)
    {
        var entity = await _context.Paises.FindAsync(id);
        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IReadOnlyCollection<Country>> FindAllAsync()
    {
        var entities = await _context.Paises
            .OrderBy(c => c.Nombre)
            .ToListAsync();
        return entities.Select(MapToDomain).ToList();
    }

    public async Task<IReadOnlyCollection<Country>> FindByContinenteAsync(int continenteId)
    {
        var entities = await _context.Paises
            .Where(c => c.ContinenteId == continenteId)
            .OrderBy(c => c.Nombre)
            .ToListAsync();
        return entities.Select(MapToDomain).ToList();
    }

    public async Task<bool> ExistsByCodigoIso2Async(string codigoIso2)
        => await _context.Paises
            .AnyAsync(c => c.CodigoIso2 == codigoIso2.ToUpperInvariant());

    public async Task<bool> ExistsByCodigoIso3Async(string codigoIso3)
        => await _context.Paises
            .AnyAsync(c => c.CodigoIso3 == codigoIso3.ToUpperInvariant());

    public async Task SaveAsync(Country country)
    {
        var entity = MapToEntity(country);
        await _context.Paises.AddAsync(entity);
        await _context.SaveChangesAsync();
        country.EstablecerId(entity.Id);
    }

    public async Task UpdateAsync(Country country)
    {
        var entity = await _context.Paises.FindAsync(country.Id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un país con ID {country.Id}.");

        entity.ContinenteId = country.ContinenteId;
        entity.Nombre       = country.Nombre.Valor;
        entity.CodigoIso2   = country.CodigoIso2.Valor;
        entity.CodigoIso3   = country.CodigoIso3.Valor;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.Paises.FindAsync(id);
        if (entity is not null)
        {
            _context.Paises.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }

    private static Country MapToDomain(CountryEntity entity)
        => Country.Reconstituir(
            entity.Id,
            entity.ContinenteId,
            entity.Nombre,
            entity.CodigoIso2,
            entity.CodigoIso3);

    private static CountryEntity MapToEntity(Country country)
        => new()
        {
            ContinenteId = country.ContinenteId,
            Nombre       = country.Nombre.Valor,
            CodigoIso2   = country.CodigoIso2.Valor,
            CodigoIso3   = country.CodigoIso3.Valor
        };
}
