// src/modules/city/Infrastructure/repository/CityRepository.cs
using Microsoft.EntityFrameworkCore;
using AirTicketSystem.shared.context;
using AirTicketSystem.modules.city.Domain.aggregate;
using AirTicketSystem.modules.city.Domain.Repositories;
using AirTicketSystem.modules.city.Infrastructure.entity;

namespace AirTicketSystem.modules.city.Infrastructure.repository;

public sealed class CityRepository : ICityRepository
{
    private readonly AppDbContext _context;

    public CityRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<City?> FindByIdAsync(int id)
    {
        var entity = await _context.Ciudades.FindAsync(id);
        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IReadOnlyCollection<City>> FindAllAsync()
    {
        var entities = await _context.Ciudades
            .OrderBy(c => c.Nombre)
            .ToListAsync();
        return entities.Select(MapToDomain).ToList();
    }

    public async Task<IReadOnlyCollection<City>> FindByDepartamentoAsync(int departamentoId)
    {
        var entities = await _context.Ciudades
            .Where(c => c.DepartamentoId == departamentoId)
            .OrderBy(c => c.Nombre)
            .ToListAsync();
        return entities.Select(MapToDomain).ToList();
    }

    public async Task<bool> ExistsByNombreAndDepartamentoAsync(string nombre, int departamentoId)
        => await _context.Ciudades
            .AnyAsync(c =>
                c.Nombre.ToLower() == nombre.ToLower() &&
                c.DepartamentoId == departamentoId);

    public async Task SaveAsync(City city)
    {
        var entity = MapToEntity(city);
        await _context.Ciudades.AddAsync(entity);
        await _context.SaveChangesAsync();
        city.EstablecerId(entity.Id);
    }

    public async Task UpdateAsync(City city)
    {
        var entity = await _context.Ciudades.FindAsync(city.Id)
            ?? throw new KeyNotFoundException(
                $"No se encontró una ciudad con ID {city.Id}.");

        entity.DepartamentoId = city.DepartamentoId;
        entity.Nombre         = city.Nombre.Valor;
        entity.CodigoPostal   = city.CodigoPostal?.Valor;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.Ciudades.FindAsync(id);
        if (entity is not null)
        {
            _context.Ciudades.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }

    private static City MapToDomain(CityEntity entity)
        => City.Reconstituir(
            entity.Id,
            entity.DepartamentoId,
            entity.Nombre,
            entity.CodigoPostal);

    private static CityEntity MapToEntity(City city)
        => new()
        {
            DepartamentoId = city.DepartamentoId,
            Nombre         = city.Nombre.Valor,
            CodigoPostal   = city.CodigoPostal?.Valor
        };
}
