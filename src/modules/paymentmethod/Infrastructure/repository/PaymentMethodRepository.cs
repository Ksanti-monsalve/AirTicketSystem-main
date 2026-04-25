// src/modules/paymentmethod/Infrastructure/repository/PaymentMethodRepository.cs
using Microsoft.EntityFrameworkCore;
using AirTicketSystem.shared.context;
using AirTicketSystem.modules.paymentmethod.Domain.aggregate;
using AirTicketSystem.modules.paymentmethod.Domain.Repositories;
using AirTicketSystem.modules.paymentmethod.Infrastructure.entity;

namespace AirTicketSystem.modules.paymentmethod.Infrastructure.repository;

public sealed class PaymentMethodRepository : IPaymentMethodRepository
{
    private readonly AppDbContext _context;

    public PaymentMethodRepository(AppDbContext context) => _context = context;

    public async Task<PaymentMethod?> FindByIdAsync(int id)
    {
        var entity = await _context.MetodosPago.FindAsync(id);
        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<IReadOnlyCollection<PaymentMethod>> FindAllAsync()
    {
        var entities = await _context.MetodosPago
            .OrderBy(pm => pm.Nombre)
            .ToListAsync();
        return entities.Select(MapToDomain).ToList();
    }

    public async Task<PaymentMethod?> FindByNombreAsync(string nombre)
    {
        var entity = await _context.MetodosPago
            .FirstOrDefaultAsync(pm =>
                pm.Nombre.ToLower() == nombre.ToLower());
        return entity is null ? null : MapToDomain(entity);
    }

    public async Task<bool> ExistsByNombreAsync(string nombre)
        => await _context.MetodosPago
            .AnyAsync(pm => pm.Nombre.ToLower() == nombre.ToLower());

    public async Task SaveAsync(PaymentMethod method)
    {
        var entity = MapToEntity(method);
        _context.MetodosPago.Add(entity);
        await _context.SaveChangesAsync();
        method.EstablecerId(entity.Id);
    }

    public async Task UpdateAsync(PaymentMethod method)
    {
        var entity = await _context.MetodosPago.FindAsync(method.Id)
            ?? throw new KeyNotFoundException(
                $"No se encontró el método de pago con ID {method.Id}.");

        entity.Nombre = method.Nombre.Valor;

        await _context.SaveChangesAsync();
    }

    private static PaymentMethod MapToDomain(PaymentMethodEntity e) =>
        PaymentMethod.Reconstituir(e.Id, e.Nombre);

    private static PaymentMethodEntity MapToEntity(PaymentMethod pm) => new()
    {
        Nombre = pm.Nombre.Valor
    };
}
