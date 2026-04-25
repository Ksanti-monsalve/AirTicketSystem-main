// src/modules/phonetype/Infrastructure/repository/PhoneTypeRepository.cs
using Microsoft.EntityFrameworkCore;
using AirTicketSystem.shared.context;
using AirTicketSystem.modules.phonetype.Domain.aggregate;
using AirTicketSystem.modules.phonetype.Domain.Repositories;
using AirTicketSystem.modules.phonetype.Infrastructure.entity;

namespace AirTicketSystem.modules.phonetype.Infrastructure.repository;

public sealed class PhoneTypeRepository : IPhoneTypeRepository
{
    private readonly AppDbContext _context;

    public PhoneTypeRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<PhoneType?> FindByIdAsync(int id)
    {
        var entity = await _context.TiposTelefono.FindAsync(id);
        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IReadOnlyCollection<PhoneType>> FindAllAsync()
    {
        var entities = await _context.TiposTelefono.OrderBy(p => p.Descripcion).ToListAsync();
        return entities.Select(MapToDomain).ToList().AsReadOnly();
    }

    public async Task<bool> ExistsByDescripcionAsync(string descripcion)
        => await _context.TiposTelefono
            .AnyAsync(p => p.Descripcion.ToLower() == descripcion.ToLower());

    public async Task SaveAsync(PhoneType phoneType)
    {
        var entity = MapToEntity(phoneType);
        await _context.TiposTelefono.AddAsync(entity);
        await _context.SaveChangesAsync();
        phoneType.EstablecerId(entity.Id);
    }

    public async Task UpdateAsync(PhoneType phoneType)
    {
        var entity = await _context.TiposTelefono.FindAsync(phoneType.Id)
            ?? throw new KeyNotFoundException(
                $"No se encontró el tipo de teléfono con ID {phoneType.Id} en la BD.");

        entity.Descripcion = phoneType.Descripcion.Valor;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.TiposTelefono.FindAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró el tipo de teléfono con ID {id} en la BD.");

        _context.TiposTelefono.Remove(entity);
        await _context.SaveChangesAsync();
    }

    private static PhoneType MapToDomain(PhoneTypeEntity entity)
        => PhoneType.Reconstituir(entity.Id, entity.Descripcion);

    private static PhoneTypeEntity MapToEntity(PhoneType phoneType)
        => new() { Descripcion = phoneType.Descripcion.Valor };
}
