// src/modules/flight/Infrastructure/repository/FlightRepository.cs
using Microsoft.EntityFrameworkCore;
using AirTicketSystem.shared.context;
using AirTicketSystem.modules.flight.Domain.Repositories;
using AirTicketSystem.modules.flight.Domain.aggregate;
using AirTicketSystem.modules.flight.Infrastructure.entity;

namespace AirTicketSystem.modules.flight.Infrastructure.repository;

public sealed class FlightRepository : IFlightRepository
{
    private readonly AppDbContext _context;

    public FlightRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Flight?> FindByIdAsync(int id)
    {
        var entity = await _context.Vuelos
            .Include(v => v.Ruta)
                .ThenInclude(r => r.Aerolinea)
            .Include(v => v.Ruta)
                .ThenInclude(r => r.Origen)
            .Include(v => v.Ruta)
                .ThenInclude(r => r.Destino)
            .Include(v => v.Avion)
                .ThenInclude(a => a.ModeloAvion)
            .FirstOrDefaultAsync(v => v.Id == id);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IReadOnlyCollection<Flight>> FindAllAsync()
    {
        var entities = await _context.Vuelos
            .Include(v => v.Ruta)
                .ThenInclude(r => r.Origen)
            .Include(v => v.Ruta)
                .ThenInclude(r => r.Destino)
            .ToListAsync();

        return entities.Select(MapToDomain).ToList().AsReadOnly();
    }

    public async Task<Flight?> FindByNumeroVueloAndFechaAsync(
        string numeroVuelo, DateTime fecha)
    {
        var entity = await _context.Vuelos
            .Include(v => v.Ruta)
                .ThenInclude(r => r.Origen)
            .Include(v => v.Ruta)
                .ThenInclude(r => r.Destino)
            .Include(v => v.Avion)
            .FirstOrDefaultAsync(v =>
                v.NumeroVuelo == numeroVuelo.ToUpperInvariant() &&
                v.FechaSalida.Date == fecha.Date);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IReadOnlyCollection<Flight>> FindByRutaAsync(int rutaId)
    {
        var entities = await _context.Vuelos
            .Where(v => v.RutaId == rutaId)
            .OrderBy(v => v.FechaSalida)
            .ToListAsync();

        return entities.Select(MapToDomain).ToList().AsReadOnly();
    }

    public async Task<IReadOnlyCollection<Flight>> FindByFechaAsync(DateTime fecha)
    {
        var entities = await _context.Vuelos
            .Include(v => v.Ruta)
                .ThenInclude(r => r.Origen)
            .Include(v => v.Ruta)
                .ThenInclude(r => r.Destino)
            .Include(v => v.Ruta)
                .ThenInclude(r => r.Aerolinea)
            .Where(v => v.FechaSalida.Date == fecha.Date)
            .OrderBy(v => v.FechaSalida)
            .ToListAsync();

        return entities.Select(MapToDomain).ToList().AsReadOnly();
    }

    public async Task<IReadOnlyCollection<Flight>> FindByOrigenAndDestinoAsync(
        int origenId, int destinoId, DateTime fecha)
    {
        var entities = await _context.Vuelos
            .Include(v => v.Ruta)
                .ThenInclude(r => r.Aerolinea)
            .Include(v => v.Ruta)
                .ThenInclude(r => r.Origen)
            .Include(v => v.Ruta)
                .ThenInclude(r => r.Destino)
            .Include(v => v.Avion)
                .ThenInclude(a => a.ModeloAvion)
            .Where(v =>
                v.Ruta.OrigenId == origenId &&
                v.Ruta.DestinoId == destinoId &&
                v.FechaSalida.Date == fecha.Date &&
                v.Estado == "PROGRAMADO")
            .OrderBy(v => v.FechaSalida)
            .ToListAsync();

        return entities.Select(MapToDomain).ToList().AsReadOnly();
    }

    public async Task<IReadOnlyCollection<Flight>> FindProgramadosAsync()
    {
        var entities = await _context.Vuelos
            .Include(v => v.Ruta)
                .ThenInclude(r => r.Origen)
            .Include(v => v.Ruta)
                .ThenInclude(r => r.Destino)
            .Where(v => v.Estado == "PROGRAMADO" &&
                        v.FechaSalida >= DateTime.UtcNow)
            .OrderBy(v => v.FechaSalida)
            .ToListAsync();

        return entities.Select(MapToDomain).ToList().AsReadOnly();
    }

    public async Task<IReadOnlyCollection<Flight>> FindConCheckinAbiertoAsync()
    {
        var entities = await _context.Vuelos
            .Where(v =>
                v.CheckinApertura != null &&
                v.CheckinCierre != null &&
                v.CheckinApertura <= DateTime.UtcNow &&
                v.CheckinCierre >= DateTime.UtcNow)
            .OrderBy(v => v.FechaSalida)
            .ToListAsync();

        return entities.Select(MapToDomain).ToList().AsReadOnly();
    }

    public async Task<bool> ExistsByNumeroVueloAndFechaAsync(
        string numeroVuelo, DateTime fecha)
        => await _context.Vuelos
            .AnyAsync(v =>
                v.NumeroVuelo == numeroVuelo.ToUpperInvariant() &&
                v.FechaSalida.Date == fecha.Date);

    public async Task SaveAsync(Flight flight)
    {
        var entity = MapToEntity(flight);
        await _context.Vuelos.AddAsync(entity);
        await _context.SaveChangesAsync();
        flight.EstablecerId(entity.Id);
    }

    public async Task UpdateAsync(Flight flight)
    {
        var entity = await _context.Vuelos.FindAsync(flight.Id)
            ?? throw new KeyNotFoundException(
                $"No se encontró el vuelo con ID {flight.Id} en la BD.");

        entity.PuertaEmbarqueId     = flight.PuertaEmbarqueId;
        entity.FechaSalida          = flight.FechaSalida.Valor;
        entity.FechaLlegadaEstimada = flight.FechaLlegadaEstimada.Valor;
        entity.FechaLlegadaReal     = flight.FechaLlegadaReal;
        entity.CheckinApertura      = flight.CheckinApertura;
        entity.CheckinCierre        = flight.CheckinCierre;
        entity.Estado               = flight.Estado.Valor;
        entity.MotivoCambioEstado   = flight.MotivoCambioEstado?.Valor;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.Vuelos.FindAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró el vuelo con ID {id} en la BD.");

        _context.Vuelos.Remove(entity);
        await _context.SaveChangesAsync();
    }

    // ── Mapeo privado ─────────────────────────────────────────

    private static Flight MapToDomain(FlightEntity entity)
        => Flight.Reconstituir(
            entity.Id,
            entity.RutaId,
            entity.AvionId,
            entity.PuertaEmbarqueId,
            entity.NumeroVuelo,
            entity.FechaSalida,
            entity.FechaLlegadaEstimada,
            entity.FechaLlegadaReal,
            entity.CheckinApertura,
            entity.CheckinCierre,
            entity.Estado,
            entity.MotivoCambioEstado);

    private static FlightEntity MapToEntity(Flight flight)
        => new FlightEntity
        {
            RutaId               = flight.RutaId,
            AvionId              = flight.AvionId,
            PuertaEmbarqueId     = flight.PuertaEmbarqueId,
            NumeroVuelo          = flight.NumeroVuelo.Valor,
            FechaSalida          = flight.FechaSalida.Valor,
            FechaLlegadaEstimada = flight.FechaLlegadaEstimada.Valor,
            FechaLlegadaReal     = flight.FechaLlegadaReal,
            CheckinApertura      = flight.CheckinApertura,
            CheckinCierre        = flight.CheckinCierre,
            Estado               = flight.Estado.Valor,
            MotivoCambioEstado   = flight.MotivoCambioEstado?.Valor
        };
}