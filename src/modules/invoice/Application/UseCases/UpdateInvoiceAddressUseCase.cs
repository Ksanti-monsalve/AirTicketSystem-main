// src/modules/invoice/Application/UseCases/UpdateInvoiceAddressUseCase.cs
using AirTicketSystem.modules.invoice.Domain.aggregate;
using AirTicketSystem.modules.invoice.Domain.Repositories;

namespace AirTicketSystem.modules.invoice.Application.UseCases;

public sealed class UpdateInvoiceAddressUseCase
{
    private readonly IInvoiceRepository _repository;

    public UpdateInvoiceAddressUseCase(IInvoiceRepository repository)
        => _repository = repository;

    public async Task<Invoice> ExecuteAsync(
        int facturaId,
        int nuevaDireccionFacturacionId,
        CancellationToken cancellationToken = default)
    {
        var invoice = await _repository.FindByIdAsync(facturaId)
            ?? throw new KeyNotFoundException(
                $"No se encontró la factura con ID {facturaId}.");

        invoice.ActualizarDireccionFacturacion(nuevaDireccionFacturacionId);

        await _repository.UpdateAsync(invoice);
        return invoice;
    }
}
