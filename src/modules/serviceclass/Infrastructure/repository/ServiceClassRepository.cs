// src/modules/serviceclass/Infrastructure/repository/ServiceClassRepository.cs
using Microsoft.EntityFrameworkCore;
using AirTicketSystem.shared.context;
using AirTicketSystem.modules.serviceclass.Domain.aggregate;
using AirTicketSystem.modules.serviceclass.Domain.Repositories;
using AirTicketSystem.modules.serviceclass.Infrastructure.entity;

namespace AirTicketSystem.modules.serviceclass.Infrastructure.repository;

public sealed class ServiceClassRepository : IServiceClassRepository
{
    private readonly AppDbContext _context;

    public ServiceClassRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ServiceClass?> FindByIdAsync(int id)
    {
        var entity = await _context.ClasesServicio.FirstOrDefaultAsync(s => s.Id == id);
        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IReadOnlyCollection<ServiceClass>> FindAllAsync()
    {
        var entities = await _context.ClasesServicio.OrderBy(s => s.Nombre).ToListAsync();
        return entities.Select(MapToDomain).ToList().AsReadOnly();
    }

    public async Task<ServiceClass?> FindByCodigoAsync(string codigo)
    {
        var entity = await _context.ClasesServicio
            .FirstOrDefaultAsync(s =>
                s.Codigo == codigo.ToUpperInvariant());
        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<bool> ExistsByCodigoAsync(string codigo)
        => await _context.ClasesServicio
            .AnyAsync(s => s.Codigo == codigo.ToUpperInvariant());

    public async Task SaveAsync(ServiceClass serviceClass)
    {
        var entity = MapToEntity(serviceClass);
        await _context.ClasesServicio.AddAsync(entity);
        await _context.SaveChangesAsync();
        serviceClass.EstablecerId(entity.Id);
    }

    public async Task UpdateAsync(ServiceClass serviceClass)
    {
        var entity = await _context.ClasesServicio.FindAsync(serviceClass.Id)
            ?? throw new KeyNotFoundException(
                $"No se encontro la clase de servicio con ID {serviceClass.Id} en la BD.");

        entity.Nombre = serviceClass.Nombre.Valor;
        entity.Descripcion = serviceClass.Descripcion?.Valor;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.ClasesServicio.FindAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontro la clase de servicio con ID {id} en la BD.");

        _context.ClasesServicio.Remove(entity);
        await _context.SaveChangesAsync();
    }

    private static ServiceClass MapToDomain(ServiceClassEntity entity)
        => ServiceClass.Reconstituir(entity.Id, entity.Nombre, entity.Codigo, entity.Descripcion);

    private static ServiceClassEntity MapToEntity(ServiceClass serviceClass)
        => new()
        {
            Nombre = serviceClass.Nombre.Valor,
            Codigo = serviceClass.Codigo.Valor,
            Descripcion = serviceClass.Descripcion?.Valor
        };
}