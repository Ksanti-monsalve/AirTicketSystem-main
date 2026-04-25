// src/modules/invoice/Domain/Repositories/IInvoiceRepository.cs
using AirTicketSystem.modules.invoice.Domain.aggregate;

namespace AirTicketSystem.modules.invoice.Domain.Repositories;

public interface IInvoiceRepository
{
    Task<Invoice?> FindByIdAsync(int id);
    Task<Invoice?> FindByReservaAsync(int reservaId);
    Task<Invoice?> FindByNumeroFacturaAsync(string numeroFactura);
    Task<IReadOnlyCollection<Invoice>> FindByFechaRangoAsync(DateTime desde, DateTime hasta);
    Task<bool> ExistsByReservaAsync(int reservaId);
    Task<bool> ExistsByNumeroFacturaAsync(string numeroFactura);
    Task SaveAsync(Invoice invoice);
    Task UpdateAsync(Invoice invoice);
}
