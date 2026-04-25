// src/modules/flightcrew/Infrastructure/repository/FlightCrewRepository.cs
using Microsoft.EntityFrameworkCore;
using AirTicketSystem.shared.context;
using AirTicketSystem.modules.flightcrew.Domain.Repositories;
using AirTicketSystem.modules.flightcrew.Domain.aggregate;
using AirTicketSystem.modules.flightcrew.Infrastructure.entity;

namespace AirTicketSystem.modules.flightcrew.Infrastructure.repository;

public sealed class FlightCrewRepository : IFlightCrewRepository
{
    private readonly AppDbContext _context;

    public FlightCrewRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<FlightCrew?> FindByIdAsync(int id)
    {
        var entity = await _context.TripulacionVuelo.FindAsync(id);
        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IReadOnlyCollection<FlightCrew>> FindByVueloAsync(int vueloId)
    {
        var entities = await _context.TripulacionVuelo
            .Include(fc => fc.Trabajador)
                .ThenInclude(w => w.Persona)
            .Where(fc => fc.VueloId == vueloId)
            .OrderBy(fc => fc.RolEnVuelo)
            .ToListAsync();

        return entities.Select(MapToDomain).ToList().AsReadOnly();
    }

    public async Task<IReadOnlyCollection<FlightCrew>> FindByTrabajadorAsync(
        int trabajadorId)
    {
        var entities = await _context.TripulacionVuelo
            .Include(fc => fc.Vuelo)
            .Where(fc => fc.TrabajadorId == trabajadorId)
            .OrderByDescending(fc => fc.Vuelo.FechaSalida)
            .ToListAsync();

        return entities.Select(MapToDomain).ToList().AsReadOnly();
    }

    public async Task<FlightCrew?> FindByVueloAndRolAsync(int vueloId, string rol)
    {
        var entity = await _context.TripulacionVuelo
            .FirstOrDefaultAsync(fc =>
                fc.VueloId == vueloId &&
                fc.RolEnVuelo == rol.ToUpperInvariant());

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<bool> VueloTienePilotoAsync(int vueloId)
        => await _context.TripulacionVuelo
            .AnyAsync(fc =>
                fc.VueloId == vueloId &&
                fc.RolEnVuelo == "PILOTO");

    public async Task<bool> VueloTieneCopiloAsync(int vueloId)
        => await _context.TripulacionVuelo
            .AnyAsync(fc =>
                fc.VueloId == vueloId &&
                fc.RolEnVuelo == "COPILOTO");

    public async Task<bool> ExistsByVueloAndTrabajadorAsync(
        int vueloId, int trabajadorId)
        => await _context.TripulacionVuelo
            .AnyAsync(fc =>
                fc.VueloId == vueloId &&
                fc.TrabajadorId == trabajadorId);

    public async Task<bool> ExistsByVueloAndRolAsync(int vueloId, string rol)
        => await _context.TripulacionVuelo
            .AnyAsync(fc =>
                fc.VueloId == vueloId &&
                fc.RolEnVuelo == rol.ToUpperInvariant());

    public async Task SaveAsync(FlightCrew flightCrew)
    {
        var entity = MapToEntity(flightCrew);
        await _context.TripulacionVuelo.AddAsync(entity);
        await _context.SaveChangesAsync();
        flightCrew.EstablecerId(entity.Id);
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.TripulacionVuelo.FindAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró la tripulación con ID {id} en la BD.");

        _context.TripulacionVuelo.Remove(entity);
        await _context.SaveChangesAsync();
    }

    private static FlightCrew MapToDomain(FlightCrewEntity entity)
        => FlightCrew.Reconstituir(
            entity.Id,
            entity.VueloId,
            entity.TrabajadorId,
            entity.RolEnVuelo);

    private static FlightCrewEntity MapToEntity(FlightCrew fc)
        => new FlightCrewEntity
        {
            VueloId      = fc.VueloId,
            TrabajadorId = fc.TrabajadorId,
            RolEnVuelo   = fc.RolEnVuelo.Valor
        };
}