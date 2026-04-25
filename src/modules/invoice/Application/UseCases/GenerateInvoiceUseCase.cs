// src/modules/invoice/Application/UseCases/GenerateInvoiceUseCase.cs
using AirTicketSystem.modules.invoice.Domain.aggregate;
using AirTicketSystem.modules.invoice.Domain.Repositories;
using AirTicketSystem.modules.booking.Domain.Repositories;

namespace AirTicketSystem.modules.invoice.Application.UseCases;

public sealed class GenerateInvoiceUseCase
{
    private readonly IInvoiceRepository _invoiceRepository;
    private readonly IBookingRepository _bookingRepository;

    public GenerateInvoiceUseCase(
        IInvoiceRepository invoiceRepository,
        IBookingRepository bookingRepository)
    {
        _invoiceRepository = invoiceRepository;
        _bookingRepository = bookingRepository;
    }

    public async Task<Invoice> ExecuteAsync(
        int reservaId,
        int direccionFacturacionId,
        decimal subtotal,
        decimal porcentajeImpuesto = 0,
        CancellationToken cancellationToken = default)
    {
        var booking = await _bookingRepository.FindByIdAsync(reservaId)
            ?? throw new KeyNotFoundException(
                $"No se encontró la reserva con ID {reservaId}.");

        if (!booking.EstaConfirmada)
            throw new InvalidOperationException(
                "Solo se puede generar factura para una reserva confirmada.");

        if (await _invoiceRepository.ExistsByReservaAsync(reservaId))
            throw new InvalidOperationException(
                "La reserva ya tiene una factura generada.");

        var invoice = Invoice.Crear(
            reservaId,
            direccionFacturacionId,
            subtotal,
            porcentajeImpuesto);

        await _invoiceRepository.SaveAsync(invoice);
        return invoice;
    }
}
