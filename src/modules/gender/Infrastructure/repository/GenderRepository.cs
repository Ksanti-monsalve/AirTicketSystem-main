// src/modules/gender/Infrastructure/repository/GenderRepository.cs
using Microsoft.EntityFrameworkCore;
using AirTicketSystem.shared.context;
using AirTicketSystem.modules.gender.Domain.aggregate;
using AirTicketSystem.modules.gender.Domain.Repositories;
using AirTicketSystem.modules.gender.Infrastructure.entity;

namespace AirTicketSystem.modules.gender.Infrastructure.repository;

public sealed class GenderRepository : IGenderRepository
{
    private readonly AppDbContext _context;

    public GenderRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Gender?> FindByIdAsync(int id)
    {
        var entity = await _context.Generos.FindAsync(id);
        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IReadOnlyCollection<Gender>> FindAllAsync()
    {
        var entities = await _context.Generos.OrderBy(g => g.Nombre).ToListAsync();
        return entities.Select(MapToDomain).ToList().AsReadOnly();
    }

    public async Task<bool> ExistsByNombreAsync(string nombre)
        => await _context.Generos
            .AnyAsync(g => g.Nombre.ToLower() == nombre.ToLower());

    public async Task SaveAsync(Gender gender)
    {
        var entity = MapToEntity(gender);
        await _context.Generos.AddAsync(entity);
        await _context.SaveChangesAsync();
        gender.EstablecerId(entity.Id);
    }

    public async Task UpdateAsync(Gender gender)
    {
        var entity = await _context.Generos.FindAsync(gender.Id)
            ?? throw new KeyNotFoundException(
                $"No se encontró el género con ID {gender.Id} en la BD.");

        entity.Nombre = gender.Nombre.Valor;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.Generos.FindAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró el género con ID {id} en la BD.");

        _context.Generos.Remove(entity);
        await _context.SaveChangesAsync();
    }

    private static Gender MapToDomain(GenderEntity entity)
        => Gender.Reconstituir(entity.Id, entity.Nombre);

    private static GenderEntity MapToEntity(Gender gender)
        => new() { Nombre = gender.Nombre.Valor };
}
