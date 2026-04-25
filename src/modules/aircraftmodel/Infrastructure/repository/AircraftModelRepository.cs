// src/modules/aircraftmodel/Infrastructure/repository/AircraftModelRepository.cs
using Microsoft.EntityFrameworkCore;
using AirTicketSystem.shared.context;
using AirTicketSystem.modules.aircraftmodel.Domain.aggregate;
using AirTicketSystem.modules.aircraftmodel.Domain.Repositories;
using AirTicketSystem.modules.aircraftmodel.Infrastructure.entity;

namespace AirTicketSystem.modules.aircraftmodel.Infrastructure.repository;

public sealed class AircraftModelRepository : IAircraftModelRepository
{
    private readonly AppDbContext _context;

    public AircraftModelRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<AircraftModel?> FindByIdAsync(int id)
    {
        var entity = await _context.ModelosAvion
            .FirstOrDefaultAsync(m => m.Id == id);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IReadOnlyCollection<AircraftModel>> FindAllAsync()
    {
        var entities = await _context.ModelosAvion
            .OrderBy(m => m.Nombre)
            .ToListAsync();
        return entities.Select(MapToDomain).ToList().AsReadOnly();
    }

    public async Task<AircraftModel?> FindByCodigoModeloAsync(string codigoModelo)
    {
        var entity = await _context.ModelosAvion
            .Include(m => m.Fabricante)
            .FirstOrDefaultAsync(m =>
                m.CodigoModelo == codigoModelo.ToUpperInvariant());
        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IReadOnlyCollection<AircraftModel>> FindByFabricanteAsync(int fabricanteId)
    {
        var entities = await _context.ModelosAvion
            .Include(m => m.Fabricante)
            .Where(m => m.FabricanteId == fabricanteId)
            .OrderBy(m => m.Nombre)
            .ToListAsync();
        return entities.Select(MapToDomain).ToList().AsReadOnly();
    }

    public async Task<bool> ExistsByCodigoModeloAsync(string codigoModelo)
        => await _context.ModelosAvion
            .AnyAsync(m => m.CodigoModelo == codigoModelo.ToUpperInvariant());

    public async Task SaveAsync(AircraftModel model)
    {
        var entity = MapToEntity(model);
        await _context.ModelosAvion.AddAsync(entity);
        await _context.SaveChangesAsync();
        model.EstablecerId(entity.Id);
    }

    public async Task UpdateAsync(AircraftModel model)
    {
        var entity = await _context.ModelosAvion.FindAsync(model.Id)
            ?? throw new KeyNotFoundException(
                $"No se encontro el modelo con ID {model.Id} en la BD.");

        entity.Nombre = model.Nombre.Valor;
        entity.AutonomiKm = model.AutonomiKm?.Valor;
        entity.VelocidadCruceroKmh = model.VelocidadCruceroKmh?.Valor;
        entity.Descripcion = model.Descripcion?.Valor;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.ModelosAvion.FindAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontro el modelo con ID {id} en la BD.");

        _context.ModelosAvion.Remove(entity);
        await _context.SaveChangesAsync();
    }

    private static AircraftModel MapToDomain(AircraftModelEntity entity)
        => AircraftModel.Reconstituir(
            entity.Id,
            entity.FabricanteId,
            entity.Nombre,
            entity.CodigoModelo,
            entity.AutonomiKm,
            entity.VelocidadCruceroKmh,
            entity.Descripcion);

    private static AircraftModelEntity MapToEntity(AircraftModel model)
        => new()
        {
            FabricanteId = model.FabricanteId,
            Nombre = model.Nombre.Valor,
            CodigoModelo = model.CodigoModelo.Valor,
            AutonomiKm = model.AutonomiKm?.Valor,
            VelocidadCruceroKmh = model.VelocidadCruceroKmh?.Valor,
            Descripcion = model.Descripcion?.Valor
        };
}