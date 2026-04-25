// src/modules/invoice/Infrastructure/repository/InvoiceRepository.cs
using Microsoft.EntityFrameworkCore;
using AirTicketSystem.shared.context;
using AirTicketSystem.modules.invoice.Domain.aggregate;
using AirTicketSystem.modules.invoice.Domain.Repositories;
using AirTicketSystem.modules.invoice.Infrastructure.entity;

namespace AirTicketSystem.modules.invoice.Infrastructure.repository;

public sealed class InvoiceRepository : IInvoiceRepository
{
    private readonly AppDbContext _context;

    public InvoiceRepository(AppDbContext context) => _context = context;

    public async Task<Invoice?> FindByIdAsync(int id)
    {
        var entity = await _context.Facturas.FindAsync(id);
        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<Invoice?> FindByReservaAsync(int reservaId)
    {
        var entity = await _context.Facturas
            .FirstOrDefaultAsync(i => i.ReservaId == reservaId);
        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<Invoice?> FindByNumeroFacturaAsync(string numeroFactura)
    {
        var entity = await _context.Facturas
            .FirstOrDefaultAsync(i =>
                i.NumeroFactura == numeroFactura.ToUpperInvariant());
        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IReadOnlyCollection<Invoice>> FindByFechaRangoAsync(
        DateTime desde, DateTime hasta)
    {
        var entities = await _context.Facturas
            .Where(i => i.FechaEmision >= desde && i.FechaEmision <= hasta)
            .OrderByDescending(i => i.FechaEmision)
            .ToListAsync();
        return entities.Select(MapToDomain).ToList();
    }

    public async Task<bool> ExistsByReservaAsync(int reservaId)
        => await _context.Facturas
            .AnyAsync(i => i.ReservaId == reservaId);

    public async Task<bool> ExistsByNumeroFacturaAsync(string numeroFactura)
        => await _context.Facturas
            .AnyAsync(i =>
                i.NumeroFactura == numeroFactura.ToUpperInvariant());

    public async Task SaveAsync(Invoice invoice)
    {
        var entity = MapToEntity(invoice);
        _context.Facturas.Add(entity);
        await _context.SaveChangesAsync();
        invoice.EstablecerId(entity.Id);
    }

    public async Task UpdateAsync(Invoice invoice)
    {
        var entity = await _context.Facturas.FindAsync(invoice.Id)
            ?? throw new KeyNotFoundException(
                $"No se encontró la factura con ID {invoice.Id}.");

        entity.DireccionFacturacionId = invoice.DireccionFacturacionId;

        await _context.SaveChangesAsync();
    }

    private static Invoice MapToDomain(InvoiceEntity e) =>
        Invoice.Reconstituir(
            e.Id,
            e.ReservaId,
            e.DireccionFacturacionId,
            e.NumeroFactura,
            e.FechaEmision,
            e.Subtotal,
            e.Impuestos,
            e.Total);

    private static InvoiceEntity MapToEntity(Invoice i) => new()
    {
        ReservaId              = i.ReservaId,
        DireccionFacturacionId = i.DireccionFacturacionId,
        NumeroFactura          = i.NumeroFactura.Valor,
        FechaEmision           = i.FechaEmision.Valor,
        Subtotal               = i.Subtotal.Valor,
        Impuestos              = i.Impuestos.Valor,
        Total                  = i.Total.Valor
    };
}
