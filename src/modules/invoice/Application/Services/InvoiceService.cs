// src/modules/invoice/Application/Services/InvoiceService.cs
using AirTicketSystem.modules.invoice.Application.Interfaces;
using AirTicketSystem.modules.invoice.Application.UseCases;
using AirTicketSystem.modules.invoice.Domain.aggregate;

namespace AirTicketSystem.modules.invoice.Application.Services;

public sealed class InvoiceService : IInvoiceService
{
    private readonly GenerateInvoiceUseCase       _generate;
    private readonly GetInvoiceByBookingUseCase   _getByBooking;
    private readonly GetInvoiceByNumeroUseCase    _getByNumero;
    private readonly UpdateInvoiceAddressUseCase  _updateAddress;

    public InvoiceService(
        GenerateInvoiceUseCase      generate,
        GetInvoiceByBookingUseCase  getByBooking,
        GetInvoiceByNumeroUseCase   getByNumero,
        UpdateInvoiceAddressUseCase updateAddress)
    {
        _generate      = generate;
        _getByBooking  = getByBooking;
        _getByNumero   = getByNumero;
        _updateAddress = updateAddress;
    }

    public Task<Invoice> GenerateAsync(
        int reservaId,
        int direccionFacturacionId,
        decimal subtotal,
        decimal porcentajeImpuesto = 0)
        => _generate.ExecuteAsync(reservaId, direccionFacturacionId, subtotal, porcentajeImpuesto);

    public Task<Invoice> GetByBookingAsync(int reservaId)
        => _getByBooking.ExecuteAsync(reservaId);

    public Task<Invoice> GetByNumeroAsync(string numeroFactura)
        => _getByNumero.ExecuteAsync(numeroFactura);

    public Task<Invoice> UpdateAddressAsync(int facturaId, int nuevaDireccionFacturacionId)
        => _updateAddress.ExecuteAsync(facturaId, nuevaDireccionFacturacionId);
}
