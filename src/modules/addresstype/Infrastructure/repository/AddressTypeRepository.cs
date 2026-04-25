// src/modules/addresstype/Infrastructure/repository/AddressTypeRepository.cs
using Microsoft.EntityFrameworkCore;
using AirTicketSystem.shared.context;
using AirTicketSystem.modules.addresstype.Domain.aggregate;
using AirTicketSystem.modules.addresstype.Domain.Repositories;
using AirTicketSystem.modules.addresstype.Infrastructure.entity;

namespace AirTicketSystem.modules.addresstype.Infrastructure.repository;

public sealed class AddressTypeRepository : IAddressTypeRepository
{
    private readonly AppDbContext _context;

    public AddressTypeRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<AddressType?> FindByIdAsync(int id)
    {
        var entity = await _context.TiposDireccion.FindAsync(id);
        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IReadOnlyCollection<AddressType>> FindAllAsync()
    {
        var entities = await _context.TiposDireccion.OrderBy(a => a.Descripcion).ToListAsync();
        return entities.Select(MapToDomain).ToList().AsReadOnly();
    }

    public async Task<bool> ExistsByDescripcionAsync(string descripcion)
        => await _context.TiposDireccion
            .AnyAsync(a => a.Descripcion.ToLower() == descripcion.ToLower());

    public async Task SaveAsync(AddressType addressType)
    {
        var entity = MapToEntity(addressType);
        await _context.TiposDireccion.AddAsync(entity);
        await _context.SaveChangesAsync();
        addressType.EstablecerId(entity.Id);
    }

    public async Task UpdateAsync(AddressType addressType)
    {
        var entity = await _context.TiposDireccion.FindAsync(addressType.Id)
            ?? throw new KeyNotFoundException(
                $"No se encontró el tipo de dirección con ID {addressType.Id} en la BD.");

        entity.Descripcion = addressType.Descripcion.Valor;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.TiposDireccion.FindAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró el tipo de dirección con ID {id} en la BD.");

        _context.TiposDireccion.Remove(entity);
        await _context.SaveChangesAsync();
    }

    private static AddressType MapToDomain(AddressTypeEntity entity)
        => AddressType.Reconstituir(entity.Id, entity.Descripcion);

    private static AddressTypeEntity MapToEntity(AddressType addressType)
        => new() { Descripcion = addressType.Descripcion.Valor };
}
