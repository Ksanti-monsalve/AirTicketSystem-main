// src/modules/additionalcharge/Infrastructure/repository/AdditionalChargeRepository.cs
using Microsoft.EntityFrameworkCore;
using AirTicketSystem.shared.context;
using AirTicketSystem.modules.additionalcharge.Domain.aggregate;
using AirTicketSystem.modules.additionalcharge.Domain.Repositories;
using AirTicketSystem.modules.additionalcharge.Infrastructure.entity;

namespace AirTicketSystem.modules.additionalcharge.Infrastructure.repository;

public sealed class AdditionalChargeRepository : IAdditionalChargeRepository
{
    private readonly AppDbContext _context;

    public AdditionalChargeRepository(AppDbContext context) => _context = context;

    public async Task<AdditionalCharge?> FindByIdAsync(int id)
    {
        var entity = await _context.CargosAdicionales.FindAsync(id);
        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IReadOnlyCollection<AdditionalCharge>> FindAllAsync()
    {
        var entities = await _context.CargosAdicionales.ToListAsync();
        return entities.Select(MapToDomain).ToList();
    }

    public async Task<IReadOnlyCollection<AdditionalCharge>> FindByReservaAsync(int reservaId)
    {
        var entities = await _context.CargosAdicionales
            .Where(ac => ac.ReservaId == reservaId)
            .OrderByDescending(ac => ac.Fecha)
            .ToListAsync();
        return entities.Select(MapToDomain).ToList();
    }

    public async Task<decimal> SumarCargosByReservaAsync(int reservaId)
        => await _context.CargosAdicionales
            .Where(ac => ac.ReservaId == reservaId)
            .SumAsync(ac => ac.Monto);

    public async Task SaveAsync(AdditionalCharge charge)
    {
        var entity = MapToEntity(charge);
        await _context.CargosAdicionales.AddAsync(entity);
        await _context.SaveChangesAsync();
        charge.EstablecerId(entity.Id);
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.CargosAdicionales.FindAsync(id);
        if (entity is not null)
        {
            _context.CargosAdicionales.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }

    private static AdditionalCharge MapToDomain(AdditionalChargeEntity entity)
        => AdditionalCharge.Reconstituir(
            entity.Id,
            entity.ReservaId,
            entity.Concepto,
            entity.Monto,
            entity.Fecha);

    private static AdditionalChargeEntity MapToEntity(AdditionalCharge charge)
        => new()
        {
            ReservaId = charge.ReservaId,
            Concepto  = charge.Concepto.Valor,
            Monto     = charge.Monto.Valor,
            Fecha     = charge.Fecha.Valor
        };
}
