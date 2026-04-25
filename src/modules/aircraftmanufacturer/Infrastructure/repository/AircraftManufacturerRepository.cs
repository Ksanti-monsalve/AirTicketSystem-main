// src/modules/aircraftmanufacturer/Infrastructure/repository/AircraftManufacturerRepository.cs
using Microsoft.EntityFrameworkCore;
using AirTicketSystem.shared.context;
using AirTicketSystem.modules.aircraftmanufacturer.Domain.aggregate;
using AirTicketSystem.modules.aircraftmanufacturer.Domain.Repositories;
using AirTicketSystem.modules.aircraftmanufacturer.Infrastructure.entity;

namespace AirTicketSystem.modules.aircraftmanufacturer.Infrastructure.repository;

public sealed class AircraftManufacturerRepository : IAircraftManufacturerRepository
{
    private readonly AppDbContext _context;

    public AircraftManufacturerRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<AircraftManufacturer?> FindByIdAsync(int id)
    {
        var entity = await _context.FabricantesAvion
            .FirstOrDefaultAsync(f => f.Id == id);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IReadOnlyCollection<AircraftManufacturer>> FindAllAsync()
    {
        var entities = await _context.FabricantesAvion
            .OrderBy(f => f.Nombre)
            .ToListAsync();

        return entities.Select(MapToDomain).ToList().AsReadOnly();
    }

    public async Task<AircraftManufacturer?> FindByNombreAsync(string nombre)
    {
        var entity = await _context.FabricantesAvion
            .Include(f => f.Pais)
            .FirstOrDefaultAsync(f =>
                f.Nombre.ToLower() == nombre.ToLower());
        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IReadOnlyCollection<AircraftManufacturer>> FindByPaisAsync(int paisId)
    {
        var entities = await _context.FabricantesAvion
            .Include(f => f.Pais)
            .Where(f => f.PaisId == paisId)
            .OrderBy(f => f.Nombre)
            .ToListAsync();
        return entities.Select(MapToDomain).ToList().AsReadOnly();
    }

    public async Task<bool> ExistsByNombreAsync(string nombre)
        => await _context.FabricantesAvion
            .AnyAsync(f => f.Nombre.ToLower() == nombre.ToLower());

    public async Task SaveAsync(AircraftManufacturer manufacturer)
    {
        var entity = MapToEntity(manufacturer);
        await _context.FabricantesAvion.AddAsync(entity);
        await _context.SaveChangesAsync();
        manufacturer.EstablecerId(entity.Id);
    }

    public async Task UpdateAsync(AircraftManufacturer manufacturer)
    {
        var entity = await _context.FabricantesAvion.FindAsync(manufacturer.Id)
            ?? throw new KeyNotFoundException(
                $"No se encontro el fabricante con ID {manufacturer.Id} en la BD.");

        entity.PaisId = manufacturer.PaisId;
        entity.Nombre = manufacturer.Nombre.Valor;
        entity.SitioWeb = manufacturer.SitioWeb?.Valor;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.FabricantesAvion.FindAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontro el fabricante con ID {id} en la BD.");

        _context.FabricantesAvion.Remove(entity);
        await _context.SaveChangesAsync();
    }

    private static AircraftManufacturer MapToDomain(AircraftManufacturerEntity entity)
        => AircraftManufacturer.Reconstituir(
            entity.Id,
            entity.PaisId,
            entity.Nombre,
            entity.SitioWeb);

    private static AircraftManufacturerEntity MapToEntity(AircraftManufacturer manufacturer)
        => new()
        {
            PaisId = manufacturer.PaisId,
            Nombre = manufacturer.Nombre.Valor,
            SitioWeb = manufacturer.SitioWeb?.Valor
        };
}