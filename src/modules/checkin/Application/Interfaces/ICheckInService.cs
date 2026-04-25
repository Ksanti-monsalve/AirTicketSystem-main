// src/modules/checkin/Application/Interfaces/ICheckInService.cs
using AirTicketSystem.modules.checkin.Domain.aggregate;

namespace AirTicketSystem.modules.checkin.Application.Interfaces;

public interface ICheckInService
{
    Task<CheckIn> CreateVirtualAsync(int pasajeroReservaId);
    Task<CheckIn> CreatePresentialAsync(int pasajeroReservaId, int trabajadorId);
    Task<CheckIn> GetByIdAsync(int id);
    Task<CheckIn> CompleteAsync(int id);
    Task<CheckIn> CancelAsync(int id);
}
