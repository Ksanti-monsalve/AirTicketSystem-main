// src/modules/checkin/Infrastructure/repository/CheckInRepository.cs
using Microsoft.EntityFrameworkCore;
using AirTicketSystem.shared.context;
using AirTicketSystem.modules.checkin.Domain.aggregate;
using AirTicketSystem.modules.checkin.Domain.Repositories;
using AirTicketSystem.modules.checkin.Infrastructure.entity;

namespace AirTicketSystem.modules.checkin.Infrastructure.repository;

public sealed class CheckInRepository : ICheckInRepository
{
    private readonly AppDbContext _context;

    public CheckInRepository(AppDbContext context) => _context = context;

    public async Task<CheckIn?> FindByIdAsync(int id)
    {
        var entity = await _context.CheckIns.FindAsync(id);
        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<CheckIn?> FindByPasajeroReservaAsync(int pasajeroReservaId)
    {
        var entity = await _context.CheckIns
            .FirstOrDefaultAsync(c => c.PasajeroReservaId == pasajeroReservaId);
        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IReadOnlyCollection<CheckIn>> FindByTrabajadorAsync(int trabajadorId)
    {
        var entities = await _context.CheckIns
            .Where(c => c.TrabajadorId == trabajadorId)
            .OrderByDescending(c => c.FechaCheckin)
            .ToListAsync();
        return entities.Select(MapToDomain).ToList();
    }

    public async Task<IReadOnlyCollection<CheckIn>> FindByEstadoAsync(string estado)
    {
        var entities = await _context.CheckIns
            .Where(c => c.Estado == estado.ToUpperInvariant())
            .OrderByDescending(c => c.FechaCheckin)
            .ToListAsync();
        return entities.Select(MapToDomain).ToList();
    }

    public async Task<IReadOnlyCollection<CheckIn>> FindByTipoAsync(string tipo)
    {
        var entities = await _context.CheckIns
            .Where(c => c.Tipo == tipo.ToUpperInvariant())
            .OrderByDescending(c => c.FechaCheckin)
            .ToListAsync();
        return entities.Select(MapToDomain).ToList();
    }

    public async Task<bool> ExistsByPasajeroReservaAsync(int pasajeroReservaId)
        => await _context.CheckIns
            .AnyAsync(c => c.PasajeroReservaId == pasajeroReservaId);

    public async Task SaveAsync(CheckIn checkIn)
    {
        var entity = MapToEntity(checkIn);
        _context.CheckIns.Add(entity);
        await _context.SaveChangesAsync();
        checkIn.EstablecerId(entity.Id);
    }

    public async Task UpdateAsync(CheckIn checkIn)
    {
        var entity = await _context.CheckIns.FindAsync(checkIn.Id)
            ?? throw new KeyNotFoundException(
                $"No se encontró el check-in con ID {checkIn.Id}.");

        entity.Estado      = checkIn.Estado.Valor;
        entity.TrabajadorId = checkIn.TrabajadorId;

        await _context.SaveChangesAsync();
    }

    private static CheckIn MapToDomain(CheckInEntity e) =>
        CheckIn.Reconstituir(
            e.Id,
            e.PasajeroReservaId,
            e.TrabajadorId,
            e.Tipo,
            e.FechaCheckin,
            e.Estado);

    private static CheckInEntity MapToEntity(CheckIn c) => new()
    {
        PasajeroReservaId = c.PasajeroReservaId,
        TrabajadorId      = c.TrabajadorId,
        Tipo              = c.Tipo.Valor,
        FechaCheckin      = c.FechaCheckin.Valor,
        Estado            = c.Estado.Valor
    };
}
