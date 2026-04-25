// src/modules/airport/Infrastructure/repository/AirportRepository.cs
using Microsoft.EntityFrameworkCore;
using AirTicketSystem.shared.context;
using AirTicketSystem.modules.airport.Domain.aggregate;
using AirTicketSystem.modules.airport.Domain.Repositories;
using AirTicketSystem.modules.airport.Infrastructure.entity;

namespace AirTicketSystem.modules.airport.Infrastructure.repository;

public sealed class AirportRepository : IAirportRepository
{
    private readonly AppDbContext _context;

    public AirportRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Airport?> FindByIdAsync(int id)
    {
        var entity = await _context.Aeropuertos
            .FirstOrDefaultAsync(a => a.Id == id);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IReadOnlyCollection<Airport>> FindAllAsync()
    {
        var entities = await _context.Aeropuertos
            .OrderBy(a => a.Nombre)
            .ToListAsync();

        return entities.Select(MapToDomain).ToList().AsReadOnly();
    }

    public async Task<Airport?> FindByCodigoIataAsync(string codigoIata)
    {
        var entity = await _context.Aeropuertos
            .Include(a => a.Ciudad)
            .FirstOrDefaultAsync(a => a.CodigoIata == codigoIata.ToUpperInvariant());
        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<Airport?> FindByCodigoIcaoAsync(string codigoIcao)
    {
        var entity = await _context.Aeropuertos
            .Include(a => a.Ciudad)
            .FirstOrDefaultAsync(a => a.CodigoIcao == codigoIcao.ToUpperInvariant());
        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IReadOnlyCollection<Airport>> FindByCiudadAsync(int ciudadId)
    {
        var entities = await _context.Aeropuertos
            .Where(a => a.CiudadId == ciudadId)
            .OrderBy(a => a.Nombre)
            .ToListAsync();
        return entities.Select(MapToDomain).ToList().AsReadOnly();
    }

    public async Task<IReadOnlyCollection<Airport>> FindActivosAsync()
    {
        var entities = await _context.Aeropuertos
            .Include(a => a.Ciudad)
            .Where(a => a.Activo)
            .OrderBy(a => a.Nombre)
            .ToListAsync();
        return entities.Select(MapToDomain).ToList().AsReadOnly();
    }

    public async Task<bool> ExistsByCodigoIataAsync(string codigoIata)
        => await _context.Aeropuertos
            .AnyAsync(a => a.CodigoIata == codigoIata.ToUpperInvariant());

    public async Task<bool> ExistsByCodigoIcaoAsync(string codigoIcao)
        => await _context.Aeropuertos
            .AnyAsync(a => a.CodigoIcao == codigoIcao.ToUpperInvariant());

    public async Task SaveAsync(Airport airport)
    {
        var entity = MapToEntity(airport);
        await _context.Aeropuertos.AddAsync(entity);
        await _context.SaveChangesAsync();
        airport.EstablecerId(entity.Id);
    }

    public async Task UpdateAsync(Airport airport)
    {
        var entity = await _context.Aeropuertos.FindAsync(airport.Id)
            ?? throw new KeyNotFoundException(
                $"No se encontro el aeropuerto con ID {airport.Id} en la BD.");

        entity.Nombre = airport.Nombre.Valor;
        entity.Direccion = airport.Direccion?.Valor;
        entity.Activo = airport.Activo.Valor;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.Aeropuertos.FindAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontro el aeropuerto con ID {id} en la BD.");

        _context.Aeropuertos.Remove(entity);
        await _context.SaveChangesAsync();
    }

    private static Airport MapToDomain(AirportEntity entity)
        => Airport.Reconstituir(
            entity.Id,
            entity.CiudadId,
            entity.CodigoIata,
            entity.CodigoIcao,
            entity.Nombre,
            entity.Direccion,
            entity.Activo);

    private static AirportEntity MapToEntity(Airport airport)
        => new()
        {
            CiudadId = airport.CiudadId,
            CodigoIata = airport.CodigoIata.Valor,
            CodigoIcao = airport.CodigoIcao.Valor,
            Nombre = airport.Nombre.Valor,
            Direccion = airport.Direccion?.Valor,
            Activo = airport.Activo.Valor
        };
}