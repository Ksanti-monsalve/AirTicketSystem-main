// src/modules/checkin/Domain/Repositories/ICheckInRepository.cs
using AirTicketSystem.modules.checkin.Domain.aggregate;

namespace AirTicketSystem.modules.checkin.Domain.Repositories;

public interface ICheckInRepository
{
    Task<CheckIn?> FindByIdAsync(int id);
    Task<CheckIn?> FindByPasajeroReservaAsync(int pasajeroReservaId);
    Task<IReadOnlyCollection<CheckIn>> FindByTrabajadorAsync(int trabajadorId);
    Task<IReadOnlyCollection<CheckIn>> FindByEstadoAsync(string estado);
    Task<IReadOnlyCollection<CheckIn>> FindByTipoAsync(string tipo);
    Task<bool> ExistsByPasajeroReservaAsync(int pasajeroReservaId);
    Task SaveAsync(CheckIn checkIn);
    Task UpdateAsync(CheckIn checkIn);
}
