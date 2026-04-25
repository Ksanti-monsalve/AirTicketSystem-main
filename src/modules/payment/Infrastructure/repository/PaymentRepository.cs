// src/modules/payment/Infrastructure/repository/PaymentRepository.cs
using Microsoft.EntityFrameworkCore;
using AirTicketSystem.shared.context;
using AirTicketSystem.modules.payment.Domain.aggregate;
using AirTicketSystem.modules.payment.Domain.Repositories;
using AirTicketSystem.modules.payment.Infrastructure.entity;

namespace AirTicketSystem.modules.payment.Infrastructure.repository;

public sealed class PaymentRepository : IPaymentRepository
{
    private readonly AppDbContext _context;

    public PaymentRepository(AppDbContext context) => _context = context;

    public async Task<Payment?> FindByIdAsync(int id)
    {
        var entity = await _context.Pagos.FindAsync(id);
        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IReadOnlyCollection<Payment>> FindByReservaAsync(int reservaId)
    {
        var entities = await _context.Pagos
            .Where(p => p.ReservaId == reservaId)
            .OrderByDescending(p => p.FechaPago)
            .ToListAsync();
        return entities.Select(MapToDomain).ToList();
    }

    public async Task<Payment?> FindAprobadoByReservaAsync(int reservaId)
    {
        var entity = await _context.Pagos
            .FirstOrDefaultAsync(p =>
                p.ReservaId == reservaId &&
                p.Estado == "APROBADO");
        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IReadOnlyCollection<Payment>> FindByEstadoAsync(string estado)
    {
        var entities = await _context.Pagos
            .Where(p => p.Estado == estado.ToUpperInvariant())
            .OrderByDescending(p => p.FechaPago)
            .ToListAsync();
        return entities.Select(MapToDomain).ToList();
    }

    public async Task<IReadOnlyCollection<Payment>> FindVencidosAsync()
    {
        var entities = await _context.Pagos
            .Where(p =>
                p.Estado == "PENDIENTE" &&
                p.FechaVencimiento <= DateTime.UtcNow)
            .ToListAsync();
        return entities.Select(MapToDomain).ToList();
    }

    public async Task<bool> ReservaTienePagoAprobadoAsync(int reservaId)
        => await _context.Pagos
            .AnyAsync(p =>
                p.ReservaId == reservaId &&
                p.Estado == "APROBADO");

    public async Task<decimal> SumarPagosAprobadosByReservaAsync(int reservaId)
        => await _context.Pagos
            .Where(p =>
                p.ReservaId == reservaId &&
                p.Estado == "APROBADO")
            .SumAsync(p => p.Monto);

    public async Task SaveAsync(Payment payment)
    {
        var entity = MapToEntity(payment);
        _context.Pagos.Add(entity);
        await _context.SaveChangesAsync();
        payment.EstablecerId(entity.Id);
    }

    public async Task UpdateAsync(Payment payment)
    {
        var entity = await _context.Pagos.FindAsync(payment.Id)
            ?? throw new KeyNotFoundException(
                $"No se encontró el pago con ID {payment.Id}.");

        entity.Estado           = payment.Estado.Valor;
        entity.ReferenciaPago   = payment.Referencia?.Valor;
        entity.FechaPago        = payment.FechaPago?.Valor;
        entity.FechaVencimiento = payment.FechaVencimiento.Valor;
        entity.MetodoPagoId     = payment.MetodoPagoId;
        entity.Monto            = payment.Monto.Valor;

        await _context.SaveChangesAsync();
    }

    private static Payment MapToDomain(PaymentEntity e) =>
        Payment.Reconstituir(
            e.Id,
            e.ReservaId,
            e.MetodoPagoId,
            e.Monto,
            e.Estado,
            e.ReferenciaPago,
            e.FechaPago,
            e.FechaVencimiento);

    private static PaymentEntity MapToEntity(Payment p) => new()
    {
        ReservaId        = p.ReservaId,
        MetodoPagoId     = p.MetodoPagoId,
        Monto            = p.Monto.Valor,
        Estado           = p.Estado.Valor,
        ReferenciaPago   = p.Referencia?.Valor,
        FechaPago        = p.FechaPago?.Valor,
        FechaVencimiento = p.FechaVencimiento.Valor
    };
}
