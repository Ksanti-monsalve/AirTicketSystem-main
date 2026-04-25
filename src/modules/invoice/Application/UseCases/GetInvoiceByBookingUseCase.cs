// src/modules/invoice/Application/UseCases/GetInvoiceByBookingUseCase.cs
using AirTicketSystem.modules.invoice.Domain.aggregate;
using AirTicketSystem.modules.invoice.Domain.Repositories;

namespace AirTicketSystem.modules.invoice.Application.UseCases;

public sealed class GetInvoiceByBookingUseCase
{
    private readonly IInvoiceRepository _repository;

    public GetInvoiceByBookingUseCase(IInvoiceRepository repository)
        => _repository = repository;

    public async Task<Invoice> ExecuteAsync(
        int reservaId,
        CancellationToken cancellationToken = default)
        => await _repository.FindByReservaAsync(reservaId)
            ?? throw new KeyNotFoundException(
                $"No se encontró una factura para la reserva con ID {reservaId}.");
}
