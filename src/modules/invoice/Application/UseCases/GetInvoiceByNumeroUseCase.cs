// src/modules/invoice/Application/UseCases/GetInvoiceByNumeroUseCase.cs
using AirTicketSystem.modules.invoice.Domain.aggregate;
using AirTicketSystem.modules.invoice.Domain.Repositories;

namespace AirTicketSystem.modules.invoice.Application.UseCases;

public sealed class GetInvoiceByNumeroUseCase
{
    private readonly IInvoiceRepository _repository;

    public GetInvoiceByNumeroUseCase(IInvoiceRepository repository)
        => _repository = repository;

    public async Task<Invoice> ExecuteAsync(
        string numeroFactura,
        CancellationToken cancellationToken = default)
        => await _repository.FindByNumeroFacturaAsync(numeroFactura)
            ?? throw new KeyNotFoundException(
                $"No se encontró la factura con número '{numeroFactura}'.");
}
