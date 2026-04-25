// src/modules/ticket/Infrastructure/repository/TicketRepository.cs
using Microsoft.EntityFrameworkCore;
using AirTicketSystem.shared.context;
using AirTicketSystem.modules.ticket.Domain.aggregate;
using AirTicketSystem.modules.ticket.Domain.Repositories;
using AirTicketSystem.modules.ticket.Infrastructure.entity;

namespace AirTicketSystem.modules.ticket.Infrastructure.repository;

public sealed class TicketRepository : ITicketRepository
{
    private readonly AppDbContext _context;

    public TicketRepository(AppDbContext context) => _context = context;

    public async Task<Ticket?> FindByIdAsync(int id)
    {
        var entity = await _context.Tiquetes.FindAsync(id);
        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<Ticket?> FindByCodigoTiqueteAsync(string codigoTiquete)
    {
        var entity = await _context.Tiquetes
            .FirstOrDefaultAsync(t => t.CodigoTiquete == codigoTiquete.ToUpperInvariant());
        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<Ticket?> FindByPasajeroReservaAsync(int pasajeroReservaId)
    {
        var entity = await _context.Tiquetes
            .FirstOrDefaultAsync(t => t.PasajeroReservaId == pasajeroReservaId);
        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IReadOnlyCollection<Ticket>> FindByEstadoAsync(string estado)
    {
        var entities = await _context.Tiquetes
            .Where(t => t.Estado == estado.ToUpperInvariant())
            .OrderByDescending(t => t.FechaEmision)
            .ToListAsync();
        return entities.Select(MapToDomain).ToList();
    }

    public async Task<IReadOnlyCollection<Ticket>> FindByVueloAsync(int vueloId)
    {
        var entities = await _context.Tiquetes
            .Where(t => t.PasajeroReserva.Reserva.VueloId == vueloId)
            .OrderByDescending(t => t.FechaEmision)
            .ToListAsync();
        return entities.Select(MapToDomain).ToList();
    }

    public async Task<bool> ExistsByCodigoTiqueteAsync(string codigoTiquete)
        => await _context.Tiquetes
            .AnyAsync(t => t.CodigoTiquete == codigoTiquete.ToUpperInvariant());

    public async Task<bool> ExistsByPasajeroReservaAsync(int pasajeroReservaId)
        => await _context.Tiquetes
            .AnyAsync(t => t.PasajeroReservaId == pasajeroReservaId);

    public async Task SaveAsync(Ticket ticket)
    {
        var entity = MapToEntity(ticket);
        _context.Tiquetes.Add(entity);
        await _context.SaveChangesAsync();
        ticket.EstablecerId(entity.Id);
    }

    public async Task UpdateAsync(Ticket ticket)
    {
        var entity = await _context.Tiquetes.FindAsync(ticket.Id)
            ?? throw new KeyNotFoundException($"No se encontró el tiquete con ID {ticket.Id}.");

        entity.Estado              = ticket.Estado.Valor;
        entity.AsientoConfirmadoId = ticket.AsientoConfirmadoId;

        await _context.SaveChangesAsync();
    }

    private static Ticket MapToDomain(TicketEntity e) =>
        Ticket.Reconstituir(
            e.Id,
            e.PasajeroReservaId,
            e.AsientoConfirmadoId,
            e.CodigoTiquete,
            e.FechaEmision,
            e.Estado);

    private static TicketEntity MapToEntity(Ticket t) => new()
    {
        PasajeroReservaId   = t.PasajeroReservaId,
        CodigoTiquete       = t.CodigoTiquete.Valor,
        AsientoConfirmadoId = t.AsientoConfirmadoId,
        FechaEmision        = t.FechaEmision.Valor,
        Estado              = t.Estado.Valor
    };
}
