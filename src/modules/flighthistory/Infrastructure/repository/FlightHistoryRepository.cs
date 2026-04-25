// src/modules/flighthistory/Infrastructure/repository/FlightHistoryRepository.cs
using Microsoft.EntityFrameworkCore;
using AirTicketSystem.shared.context;
using AirTicketSystem.modules.flighthistory.Domain.Repositories;
using AirTicketSystem.modules.flighthistory.Domain.aggregate;
using AirTicketSystem.modules.flighthistory.Infrastructure.entity;

namespace AirTicketSystem.modules.flighthistory.Infrastructure.repository;

public sealed class FlightHistoryRepository : IFlightHistoryRepository
{
    private readonly AppDbContext _context;

    public FlightHistoryRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyCollection<FlightHistory>> FindByVueloAsync(int vueloId)
    {
        var entities = await _context.HistorialVuelo
            .Where(h => h.VueloId == vueloId)
            .OrderByDescending(h => h.FechaCambio)
            .ToListAsync();

        return entities.Select(MapToDomain).ToList().AsReadOnly();
    }

    public async Task<FlightHistory?> FindUltimoCambioByVueloAsync(int vueloId)
    {
        var entity = await _context.HistorialVuelo
            .Where(h => h.VueloId == vueloId)
            .OrderByDescending(h => h.FechaCambio)
            .FirstOrDefaultAsync();

        return entity is null ? null : MapToDomain(entity);
    }

    public async Task SaveAsync(FlightHistory flightHistory)
    {
        var entity = MapToEntity(flightHistory);
        await _context.HistorialVuelo.AddAsync(entity);
        await _context.SaveChangesAsync();
        flightHistory.EstablecerId(entity.Id);
    }

    private static FlightHistory MapToDomain(FlightHistoryEntity entity)
        => FlightHistory.Reconstituir(
            entity.Id,
            entity.VueloId,
            entity.UsuarioId,
            entity.EstadoAnterior,
            entity.EstadoNuevo,
            entity.FechaCambio,
            entity.Motivo);

    private static FlightHistoryEntity MapToEntity(FlightHistory fh)
        => new FlightHistoryEntity
        {
            VueloId        = fh.VueloId,
            UsuarioId      = fh.UsuarioId,
            EstadoAnterior = fh.EstadoAnterior.Valor,
            EstadoNuevo    = fh.EstadoNuevo.Valor,
            FechaCambio    = fh.FechaCambio,
            Motivo         = fh.Motivo?.Valor
        };
}