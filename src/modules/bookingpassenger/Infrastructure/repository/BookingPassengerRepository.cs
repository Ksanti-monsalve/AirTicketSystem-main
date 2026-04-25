// src/modules/bookingpassenger/Infrastructure/repository/BookingPassengerRepository.cs
using Microsoft.EntityFrameworkCore;
using AirTicketSystem.shared.context;
using AirTicketSystem.modules.bookingpassenger.Domain.aggregate;
using AirTicketSystem.modules.bookingpassenger.Domain.Repositories;
using AirTicketSystem.modules.bookingpassenger.Infrastructure.entity;

namespace AirTicketSystem.modules.bookingpassenger.Infrastructure.repository;

public sealed class BookingPassengerRepository : IBookingPassengerRepository
{
    private readonly AppDbContext _context;

    public BookingPassengerRepository(AppDbContext context) => _context = context;

    public async Task<BookingPassenger?> FindByIdAsync(int id)
    {
        var entity = await _context.PasajerosReserva.FindAsync(id);
        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IReadOnlyCollection<BookingPassenger>> FindByReservaAsync(int reservaId)
    {
        var entities = await _context.PasajerosReserva
            .Where(bp => bp.ReservaId == reservaId)
            .ToListAsync();
        return entities.Select(MapToDomain).ToList();
    }

    public async Task<BookingPassenger?> FindByReservaAndPersonaAsync(int reservaId, int personaId)
    {
        var entity = await _context.PasajerosReserva
            .FirstOrDefaultAsync(bp => bp.ReservaId == reservaId && bp.PersonaId == personaId);
        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IReadOnlyCollection<BookingPassenger>> FindByPersonaAsync(int personaId)
    {
        var entities = await _context.PasajerosReserva
            .Where(bp => bp.PersonaId == personaId)
            .ToListAsync();
        return entities.Select(MapToDomain).ToList();
    }

    public async Task<bool> ExistsByReservaAndPersonaAsync(int reservaId, int personaId)
        => await _context.PasajerosReserva
            .AnyAsync(bp => bp.ReservaId == reservaId && bp.PersonaId == personaId);

    public async Task<int> ContarByReservaAsync(int reservaId)
        => await _context.PasajerosReserva
            .CountAsync(bp => bp.ReservaId == reservaId);

    public async Task SaveAsync(BookingPassenger passenger)
    {
        var entity = MapToEntity(passenger);
        _context.PasajerosReserva.Add(entity);
        await _context.SaveChangesAsync();
        passenger.EstablecerId(entity.Id);
    }

    public async Task UpdateAsync(BookingPassenger passenger)
    {
        var entity = await _context.PasajerosReserva.FindAsync(passenger.Id)
            ?? throw new KeyNotFoundException(
                $"No se encontró el pasajero de reserva con ID {passenger.Id}.");

        entity.AsientoId = passenger.AsientoId;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.PasajerosReserva.FindAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró el pasajero de reserva con ID {id}.");
        _context.PasajerosReserva.Remove(entity);
        await _context.SaveChangesAsync();
    }

    private static BookingPassenger MapToDomain(BookingPassengerEntity e) =>
        BookingPassenger.Reconstituir(
            e.Id,
            e.ReservaId,
            e.PersonaId,
            e.TipoPasajero,
            e.AsientoId);

    private static BookingPassengerEntity MapToEntity(BookingPassenger p) => new()
    {
        ReservaId    = p.ReservaId,
        PersonaId    = p.PersonaId,
        TipoPasajero = p.TipoPasajero.Valor,
        AsientoId    = p.AsientoId
    };
}
