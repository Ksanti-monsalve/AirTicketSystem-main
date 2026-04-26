// src/modules/seatavailability/Infrastructure/repository/SeatAvailabilityRepository.cs
using Microsoft.EntityFrameworkCore;
using AirTicketSystem.shared.context;
using AirTicketSystem.modules.seatavailability.Domain.Repositories;
using AirTicketSystem.modules.seatavailability.Domain.aggregate;
using AirTicketSystem.modules.seatavailability.Infrastructure.entity;

namespace AirTicketSystem.modules.seatavailability.Infrastructure.repository;

public sealed class SeatAvailabilityRepository : ISeatAvailabilityRepository
{
    private readonly AppDbContext _context;

    public SeatAvailabilityRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyCollection<SeatAvailabilityDetail>> FindDetallesByVueloAsync(
        int vueloId, string? estado = null)
    {
        var query = _context.DisponibilidadAsientos
            .AsNoTracking()
            .Where(sa => sa.VueloId == vueloId);

        if (!string.IsNullOrWhiteSpace(estado))
            query = query.Where(sa => sa.Estado == estado);

        var items = await query
            .OrderBy(sa => sa.Asiento.Fila)
            .ThenBy(sa => sa.Asiento.Columna)
            .Select(sa => new SeatAvailabilityDetail(
                sa.Id,
                sa.VueloId,
                sa.AsientoId,
                sa.NumeroAsiento,
                sa.ClaseVueloId,
                sa.Asiento.ClaseServicio.Nombre,
                sa.Estado))
            .ToListAsync();

        return items.AsReadOnly();
    }

    public async Task<IReadOnlyCollection<SeatAvailabilityDetail>> FindDetallesByReservaAsync(
        int reservaId)
    {
        var items = await _context.DisponibilidadAsientos
            .AsNoTracking()
            .Where(sa => sa.ReservaId == reservaId)
            .OrderBy(sa => sa.Asiento.Fila)
            .ThenBy(sa => sa.Asiento.Columna)
            .Select(sa => new SeatAvailabilityDetail(
                sa.Id,
                sa.VueloId,
                sa.AsientoId,
                sa.NumeroAsiento,
                sa.ClaseVueloId,
                sa.Asiento.ClaseServicio.Nombre,
                sa.Estado))
            .ToListAsync();

        return items.AsReadOnly();
    }

    public async Task<IReadOnlyCollection<ServiceClassAvailability>> FindClasesDisponiblesByVueloAsync(
        int vueloId)
    {
        var items = await _context.DisponibilidadAsientos
            .AsNoTracking()
            .Where(sa => sa.VueloId == vueloId && sa.Estado == "DISPONIBLE")
            .GroupBy(sa => new
            {
                sa.ClaseVueloId,
                sa.Asiento.ClaseServicio.Nombre,
                sa.Asiento.ClaseServicio.Codigo
            })
            .Select(g => new ServiceClassAvailability(
                g.Key.ClaseVueloId,
                g.Key.Nombre,
                g.Key.Codigo,
                g.Count()))
            .OrderBy(x => x.ClaseServicioNombre)
            .ToListAsync();

        return items.AsReadOnly();
    }

    public async Task<IReadOnlyCollection<SeatAvailabilityDetail>> FindDetallesDisponiblesByVueloAndClaseAsync(
        int vueloId, int claseServicioId)
    {
        var items = await _context.DisponibilidadAsientos
            .AsNoTracking()
            .Where(sa =>
                sa.VueloId == vueloId &&
                sa.Estado == "DISPONIBLE" &&
                sa.ClaseVueloId == claseServicioId)
            .OrderBy(sa => sa.Asiento.Fila)
            .ThenBy(sa => sa.Asiento.Columna)
            .Select(sa => new SeatAvailabilityDetail(
                sa.Id,
                sa.VueloId,
                sa.AsientoId,
                sa.NumeroAsiento,
                sa.ClaseVueloId,
                sa.Asiento.ClaseServicio.Nombre,
                sa.Estado))
            .ToListAsync();

        return items.AsReadOnly();
    }

    public async Task<SeatAvailability?> FindByVueloAndAsientoAsync(
        int vueloId, int asientoId)
    {
        var entity = await _context.DisponibilidadAsientos
            .Include(sa => sa.Asiento)
                .ThenInclude(a => a.ClaseServicio)
            .FirstOrDefaultAsync(sa =>
                sa.VueloId == vueloId &&
                sa.AsientoId == asientoId);

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IReadOnlyCollection<SeatAvailability>> FindByVueloAsync(int vueloId)
    {
        var entities = await _context.DisponibilidadAsientos
            .Include(sa => sa.Asiento)
                .ThenInclude(a => a.ClaseServicio)
            .Where(sa => sa.VueloId == vueloId)
            .OrderBy(sa => sa.Asiento.Fila)
            .ThenBy(sa => sa.Asiento.Columna)
            .ToListAsync();

        return entities.Select(MapToDomain).ToList().AsReadOnly();
    }

    public async Task<IReadOnlyCollection<SeatAvailability>> FindDisponiblesByVueloAsync(
        int vueloId)
    {
        var entities = await _context.DisponibilidadAsientos
            .Include(sa => sa.Asiento)
                .ThenInclude(a => a.ClaseServicio)
            .Where(sa => sa.VueloId == vueloId && sa.Estado == "DISPONIBLE")
            .OrderBy(sa => sa.Asiento.Fila)
            .ThenBy(sa => sa.Asiento.Columna)
            .ToListAsync();

        return entities.Select(MapToDomain).ToList().AsReadOnly();
    }

    public async Task<IReadOnlyCollection<SeatAvailability>>
        FindDisponiblesByVueloAndClaseAsync(int vueloId, int claseServicioId)
    {
        var entities = await _context.DisponibilidadAsientos
            .Include(sa => sa.Asiento)
            .Where(sa =>
                sa.VueloId == vueloId &&
                sa.Estado == "DISPONIBLE" &&
                sa.ClaseVueloId == claseServicioId)
            .OrderBy(sa => sa.Asiento.Fila)
            .ThenBy(sa => sa.Asiento.Columna)
            .ToListAsync();

        return entities.Select(MapToDomain).ToList().AsReadOnly();
    }

    public async Task<int> ContarDisponiblesByVueloAsync(int vueloId)
        => await _context.DisponibilidadAsientos
            .CountAsync(sa =>
                sa.VueloId == vueloId &&
                sa.Estado == "DISPONIBLE");

    public async Task<bool> AsientoDisponibleAsync(int vueloId, int asientoId)
        => await _context.DisponibilidadAsientos
            .AnyAsync(sa =>
                sa.VueloId == vueloId &&
                sa.AsientoId == asientoId &&
                sa.Estado == "DISPONIBLE");

    public async Task<bool> TryReserveDisponibilidadAsync(int disponibilidadId)
    {
        // Reserva atómica: solo cambia a RESERVADO si actualmente está DISPONIBLE.
        var affected = await _context.DisponibilidadAsientos
            .Where(sa => sa.Id == disponibilidadId && sa.Estado == "DISPONIBLE")
            .ExecuteUpdateAsync(setters =>
                setters.SetProperty(sa => sa.Estado, "RESERVADO"));

        return affected == 1;
    }

    public async Task<bool> TryOccupyDisponibilidadAsync(int disponibilidadId)
    {
        // Ocupación atómica: solo cambia a OCUPADO si actualmente está RESERVADO.
        var affected = await _context.DisponibilidadAsientos
            .Where(sa => sa.Id == disponibilidadId && sa.Estado == "RESERVADO")
            .ExecuteUpdateAsync(setters =>
                setters.SetProperty(sa => sa.Estado, "OCUPADO"));

        return affected == 1;
    }

    public async Task SetReservaIdAsync(int disponibilidadId, int reservaId)
    {
        if (reservaId <= 0)
            throw new ArgumentException("El ID de la reserva no es válido.");

        _ = await _context.DisponibilidadAsientos
            .Where(sa => sa.Id == disponibilidadId)
            .ExecuteUpdateAsync(setters =>
                setters.SetProperty(sa => sa.ReservaId, reservaId));
    }

    public async Task SetTiqueteIdAsync(int disponibilidadId, int tiqueteId)
    {
        if (tiqueteId <= 0)
            throw new ArgumentException("El ID del tiquete no es válido.");

        _ = await _context.DisponibilidadAsientos
            .Where(sa => sa.Id == disponibilidadId)
            .ExecuteUpdateAsync(setters =>
                setters.SetProperty(sa => sa.TiqueteId, tiqueteId));
    }

    public async Task<SeatAvailabilityDetail?> FindDetalleByDisponibilidadIdAsync(int disponibilidadId)
    {
        return await _context.DisponibilidadAsientos
            .AsNoTracking()
            .Where(sa => sa.Id == disponibilidadId)
            .Select(sa => new SeatAvailabilityDetail(
                sa.Id,
                sa.VueloId,
                sa.AsientoId,
                sa.NumeroAsiento,
                sa.ClaseVueloId,
                sa.Asiento.ClaseServicio.Nombre,
                sa.Estado))
            .FirstOrDefaultAsync();
    }

    public async Task<IReadOnlyCollection<ServiceClassStateStats>> FindStatsByVueloAsync(int vueloId)
    {
        var items = await _context.DisponibilidadAsientos
            .AsNoTracking()
            .Where(sa => sa.VueloId == vueloId)
            .GroupBy(sa => new
            {
                sa.ClaseVueloId,
                sa.Asiento.ClaseServicio.Nombre,
                sa.Asiento.ClaseServicio.Codigo
            })
            .Select(g => new ServiceClassStateStats(
                g.Key.ClaseVueloId,
                g.Key.Nombre,
                g.Key.Codigo,
                g.Count(),
                g.Count(x => x.Estado == "DISPONIBLE"),
                g.Count(x => x.Estado == "RESERVADO"),
                g.Count(x => x.Estado == "OCUPADO"),
                g.Count(x => x.Estado == "BLOQUEADO")))
            .OrderBy(x => x.ClaseServicioNombre)
            .ToListAsync();

        return items.AsReadOnly();
    }

    public async Task SaveAllAsync(IEnumerable<SeatAvailability> asientos)
    {
        // NOTA: NumeroAsiento y ClaseVueloId deben venir informados desde el caso de uso (crear vuelo)
        // ya que SeatAvailability (dominio) representa la disponibilidad, no el catálogo del asiento.
        var entities = asientos.Select(MapToEntity).ToList();
        await _context.DisponibilidadAsientos.AddRangeAsync(entities);
        await _context.SaveChangesAsync();

        // Propagar los Ids generados
        var lista = asientos.ToList();
        for (int i = 0; i < lista.Count; i++)
            lista[i].EstablecerId(entities[i].Id);
    }

    public async Task UpdateAsync(SeatAvailability seatAvailability)
    {
        var entity = await _context.DisponibilidadAsientos
            .FindAsync(seatAvailability.Id)
            ?? throw new KeyNotFoundException(
                $"No se encontró la disponibilidad con ID {seatAvailability.Id}.");

        entity.Estado = seatAvailability.Estado.Valor;
        await _context.SaveChangesAsync();
    }

    private static SeatAvailability MapToDomain(SeatAvailabilityEntity entity)
        => SeatAvailability.Reconstituir(
            entity.Id,
            entity.VueloId,
            entity.AsientoId,
            entity.NumeroAsiento,
            entity.ClaseVueloId,
            entity.Estado);

    private static SeatAvailabilityEntity MapToEntity(SeatAvailability sa)
        => new SeatAvailabilityEntity
        {
            VueloId   = sa.VueloId,
            AsientoId = sa.AsientoId,
            NumeroAsiento = sa.NumeroAsiento,
            ClaseVueloId = sa.ClaseVueloId,
            Estado    = sa.Estado.Valor
        };
}