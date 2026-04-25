// src/modules/airline/Infrastructure/repository/AirlineRepository.cs
using Microsoft.EntityFrameworkCore;
using AirTicketSystem.shared.context;
using AirTicketSystem.modules.airline.Domain.aggregate;
using AirTicketSystem.modules.airline.Domain.Repositories;
using AirTicketSystem.modules.airline.Infrastructure.entity;

namespace AirTicketSystem.modules.airline.Infrastructure.repository;

public sealed class AirlineRepository : IAirlineRepository
{
    private readonly AppDbContext _context;

    public AirlineRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Airline?> FindByIdAsync(int id)
    {
        var entity = await _context.Aerolineas
            .FirstOrDefaultAsync(a => a.Id == id);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IReadOnlyCollection<Airline>> FindAllAsync()
    {
        var entities = await _context.Aerolineas
            .OrderBy(a => a.Nombre)
            .ToListAsync();
        return entities.Select(MapToDomain).ToList().AsReadOnly();
    }

    public async Task<Airline?> FindByCodigoIataAsync(string codigoIata)
    {
        var entity = await _context.Aerolineas
            .Include(a => a.Pais)
            .FirstOrDefaultAsync(a =>
                a.CodigoIata == codigoIata.ToUpperInvariant());
        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<Airline?> FindByCodigoIcaoAsync(string codigoIcao)
    {
        var entity = await _context.Aerolineas
            .Include(a => a.Pais)
            .FirstOrDefaultAsync(a =>
                a.CodigoIcao == codigoIcao.ToUpperInvariant());
        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IReadOnlyCollection<Airline>> FindActivasAsync()
    {
        var entities = await _context.Aerolineas
            .Include(a => a.Pais)
            .Where(a => a.Activa)
            .OrderBy(a => a.Nombre)
            .ToListAsync();
        return entities.Select(MapToDomain).ToList().AsReadOnly();
    }

    public async Task<bool> ExistsByCodigoIataAsync(string codigoIata)
        => await _context.Aerolineas
            .AnyAsync(a => a.CodigoIata == codigoIata.ToUpperInvariant());

    public async Task<bool> ExistsByCodigoIcaoAsync(string codigoIcao)
        => await _context.Aerolineas
            .AnyAsync(a => a.CodigoIcao == codigoIcao.ToUpperInvariant());

    public async Task SaveAsync(Airline airline)
    {
        var entity = MapToEntity(airline);
        await _context.Aerolineas.AddAsync(entity);
        await _context.SaveChangesAsync();
        airline.EstablecerId(entity.Id);
    }

    public async Task UpdateAsync(Airline airline)
    {
        var entity = await _context.Aerolineas.FindAsync(airline.Id)
            ?? throw new KeyNotFoundException(
                $"No se encontro la aerolinea con ID {airline.Id} en la BD.");

        entity.PaisId = airline.PaisId;
        entity.Nombre = airline.Nombre.Valor;
        entity.NombreComercial = airline.NombreComercial?.Valor;
        entity.SitioWeb = airline.SitioWeb?.Valor;
        entity.Activa = airline.Activa.Valor;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.Aerolineas.FindAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontro la aerolinea con ID {id} en la BD.");
        _context.Aerolineas.Remove(entity);
        await _context.SaveChangesAsync();
    }

    private static Airline MapToDomain(AirlineEntity entity)
        => Airline.Reconstituir(
            entity.Id,
            entity.PaisId,
            entity.CodigoIata,
            entity.CodigoIcao,
            entity.Nombre,
            entity.NombreComercial,
            entity.SitioWeb,
            entity.Activa);

    private static AirlineEntity MapToEntity(Airline airline)
        => new()
        {
            PaisId = airline.PaisId,
            CodigoIata = airline.CodigoIata.Valor,
            CodigoIcao = airline.CodigoIcao.Valor,
            Nombre = airline.Nombre.Valor,
            NombreComercial = airline.NombreComercial?.Valor,
            SitioWeb = airline.SitioWeb?.Valor,
            Activa = airline.Activa.Valor
        };
}