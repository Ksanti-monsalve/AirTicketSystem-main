// src/modules/booking/Infrastructure/repository/BookingRepository.cs
using Microsoft.EntityFrameworkCore;
using AirTicketSystem.shared.context;
using AirTicketSystem.modules.booking.Domain.aggregate;
using AirTicketSystem.modules.booking.Domain.Repositories;
using AirTicketSystem.modules.booking.Infrastructure.entity;

namespace AirTicketSystem.modules.booking.Infrastructure.repository;

public sealed class BookingRepository : IBookingRepository
{
    private readonly AppDbContext _context;

    public BookingRepository(AppDbContext context) => _context = context;

    public async Task<Booking?> FindByIdAsync(int id)
    {
        var entity = await _context.Reservas.FindAsync(id);
        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IReadOnlyCollection<Booking>> FindAllAsync()
    {
        var entities = await _context.Reservas
            .OrderByDescending(b => b.FechaReserva)
            .ToListAsync();
        return entities.Select(MapToDomain).ToList();
    }

    public async Task<Booking?> FindByCodigoReservaAsync(string codigoReserva)
    {
        var entity = await _context.Reservas
            .FirstOrDefaultAsync(b => b.CodigoReserva == codigoReserva.ToUpperInvariant());
        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IReadOnlyCollection<Booking>> FindByClienteAsync(int clienteId)
    {
        var entities = await _context.Reservas
            .Where(b => b.ClienteId == clienteId)
            .OrderByDescending(b => b.FechaReserva)
            .ToListAsync();
        return entities.Select(MapToDomain).ToList();
    }

    public async Task<IReadOnlyCollection<Booking>> FindByVueloAsync(int vueloId)
    {
        var entities = await _context.Reservas
            .Where(b => b.VueloId == vueloId)
            .OrderBy(b => b.FechaReserva)
            .ToListAsync();
        return entities.Select(MapToDomain).ToList();
    }

    public async Task<IReadOnlyCollection<Booking>> FindByEstadoAsync(string estado)
    {
        var entities = await _context.Reservas
            .Where(b => b.Estado == estado.ToUpperInvariant())
            .OrderByDescending(b => b.FechaReserva)
            .ToListAsync();
        return entities.Select(MapToDomain).ToList();
    }

    public async Task<IReadOnlyCollection<Booking>> FindExpirasAsync()
    {
        var entities = await _context.Reservas
            .Where(b => b.Estado == "PENDIENTE" && b.FechaExpiracion <= DateTime.UtcNow)
            .ToListAsync();
        return entities.Select(MapToDomain).ToList();
    }

    public async Task<bool> ExistsByCodigoReservaAsync(string codigoReserva)
        => await _context.Reservas
            .AnyAsync(b => b.CodigoReserva == codigoReserva.ToUpperInvariant());

    public async Task<int> ContarByVueloAsync(int vueloId)
        => await _context.Reservas
            .CountAsync(b =>
                b.VueloId == vueloId &&
                b.Estado != "CANCELADA" &&
                b.Estado != "EXPIRADA");

    public async Task SaveAsync(Booking booking)
    {
        var entity = MapToEntity(booking);
        _context.Reservas.Add(entity);
        await _context.SaveChangesAsync();
        booking.EstablecerId(entity.Id);
    }

    public async Task UpdateAsync(Booking booking)
    {
        var entity = await _context.Reservas.FindAsync(booking.Id)
            ?? throw new KeyNotFoundException($"No se encontró la reserva con ID {booking.Id}.");

        entity.Estado          = booking.Estado.Valor;
        entity.FechaExpiracion = booking.FechaExpiracion.Valor;
        entity.Observaciones   = booking.Observaciones?.Valor;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.Reservas.FindAsync(id)
            ?? throw new KeyNotFoundException($"No se encontró la reserva con ID {id}.");
        _context.Reservas.Remove(entity);
        await _context.SaveChangesAsync();
    }

    private static Booking MapToDomain(BookingEntity e) =>
        Booking.Reconstituir(
            e.Id,
            e.ClienteId,
            e.VueloId,
            e.TarifaId,
            e.CodigoReserva,
            e.FechaReserva,
            e.FechaExpiracion,
            e.Estado,
            e.ValorTotal,
            e.Observaciones);

    private static BookingEntity MapToEntity(Booking b) => new()
    {
        ClienteId       = b.ClienteId,
        VueloId         = b.VueloId,
        TarifaId        = b.TarifaId,
        CodigoReserva   = b.CodigoReserva.Valor,
        FechaReserva    = b.FechaReserva.Valor,
        FechaExpiracion = b.FechaExpiracion.Valor,
        Estado          = b.Estado.Valor,
        ValorTotal      = b.ValorTotal.Valor,
        Observaciones   = b.Observaciones?.Valor
    };
}
