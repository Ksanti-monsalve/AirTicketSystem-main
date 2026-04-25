// src/modules/additionalcharge/Domain/Repositories/IAdditionalChargeRepository.cs
using AirTicketSystem.modules.additionalcharge.Domain.aggregate;

namespace AirTicketSystem.modules.additionalcharge.Domain.Repositories;

public interface IAdditionalChargeRepository
{
    Task<AdditionalCharge?> FindByIdAsync(int id);
    Task<IReadOnlyCollection<AdditionalCharge>> FindAllAsync();
    Task<IReadOnlyCollection<AdditionalCharge>> FindByReservaAsync(int reservaId);
    Task<decimal> SumarCargosByReservaAsync(int reservaId);
    Task SaveAsync(AdditionalCharge charge);
    Task DeleteAsync(int id);
}
