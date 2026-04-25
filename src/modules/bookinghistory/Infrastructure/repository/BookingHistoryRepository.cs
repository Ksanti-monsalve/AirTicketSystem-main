// src/modules/bookinghistory/Infrastructure/repository/BookingHistoryRepository.cs
using Microsoft.EntityFrameworkCore;
using AirTicketSystem.shared.context;
using AirTicketSystem.modules.bookinghistory.Domain.aggregate;
using AirTicketSystem.modules.bookinghistory.Domain.Repositories;
using AirTicketSystem.modules.bookinghistory.Infrastructure.entity;

namespace AirTicketSystem.modules.bookinghistory.Infrastructure.repository;

public sealed class BookingHistoryRepository : IBookingHistoryRepository
{
    private readonly AppDbContext _context;

    public BookingHistoryRepository(AppDbContext context) => _context = context;

    public async Task<BookingHistory?> FindByIdAsync(int id)
    {
        var entity = await _context.HistorialReserva.FindAsync(id);
        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IReadOnlyCollection<BookingHistory>> FindByReservaAsync(int reservaId)
    {
        var entities = await _context.HistorialReserva
            .Where(h => h.ReservaId == reservaId)
            .OrderByDescending(h => h.FechaCambio)
            .ToListAsync();
        return entities.Select(MapToDomain).ToList();
    }

    public async Task<BookingHistory?> FindUltimoCambioByReservaAsync(int reservaId)
    {
        var entity = await _context.HistorialReserva
            .Where(h => h.ReservaId == reservaId)
            .OrderByDescending(h => h.FechaCambio)
            .FirstOrDefaultAsync();
        return entity is null ? null : MapToDomain(entity);
    }

    public async Task SaveAsync(BookingHistory history)
    {
        var entity = MapToEntity(history);
        _context.HistorialReserva.Add(entity);
        await _context.SaveChangesAsync();
        history.EstablecerId(entity.Id);
    }

    private static BookingHistory MapToDomain(BookingHistoryEntity e) =>
        BookingHistory.Reconstituir(
            e.Id,
            e.ReservaId,
            e.EstadoAnterior,
            e.EstadoNuevo,
            e.FechaCambio,
            e.UsuarioId,
            e.Motivo);

    private static BookingHistoryEntity MapToEntity(BookingHistory h) => new()
    {
        ReservaId      = h.ReservaId,
        EstadoAnterior = h.EstadoAnterior.Valor,
        EstadoNuevo    = h.EstadoNuevo.Valor,
        FechaCambio    = h.FechaCambio.Valor,
        UsuarioId      = h.UsuarioId,
        Motivo         = h.Motivo?.Valor
    };
}
