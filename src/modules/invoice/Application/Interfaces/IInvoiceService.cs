// src/modules/invoice/Application/Interfaces/IInvoiceService.cs
using AirTicketSystem.modules.invoice.Domain.aggregate;

namespace AirTicketSystem.modules.invoice.Application.Interfaces;

public interface IInvoiceService
{
    Task<Invoice> GenerateAsync(int reservaId, int direccionFacturacionId, decimal subtotal, decimal porcentajeImpuesto = 0);
    Task<Invoice> GetByBookingAsync(int reservaId);
    Task<Invoice> GetByNumeroAsync(string numeroFactura);
    Task<Invoice> UpdateAddressAsync(int facturaId, int nuevaDireccionFacturacionId);
}
