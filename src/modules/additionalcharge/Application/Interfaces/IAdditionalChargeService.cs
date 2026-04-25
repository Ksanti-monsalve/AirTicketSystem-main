// src/modules/additionalcharge/Application/Interfaces/IAdditionalChargeService.cs
using AirTicketSystem.modules.additionalcharge.Domain.aggregate;

namespace AirTicketSystem.modules.additionalcharge.Application.Interfaces;

public interface IAdditionalChargeService
{
    Task<AdditionalCharge> CreateAsync(int reservaId, string concepto, decimal monto);
    Task<AdditionalCharge> GetByIdAsync(int id);
    Task<IReadOnlyCollection<AdditionalCharge>> GetAllAsync();
    Task<IReadOnlyCollection<AdditionalCharge>> GetByBookingAsync(int reservaId);
    Task<decimal> GetTotalByBookingAsync(int reservaId);
    Task DeleteAsync(int id);
}
