// src/modules/aircraftseat/Domain/Repositories/IAircraftSeatRepository.cs
using AirTicketSystem.modules.aircraftseat.Domain.aggregate;

namespace AirTicketSystem.modules.aircraftseat.Domain.Repositories;

public interface IAircraftSeatRepository
{
    Task<AircraftSeat?> FindByIdAsync(int id);
    Task<AircraftSeat?> FindByCodigoAndAvionAsync(
        string codigoAsiento, int avionId);
    Task<IReadOnlyCollection<AircraftSeat>> FindByAvionAsync(int avionId);
    Task<IReadOnlyCollection<AircraftSeat>> FindByAvionAndClaseAsync(
        int avionId, int claseServicioId);
    Task<int> ContarAsientosByAvionAsync(int avionId);
    Task<bool> ExistsByCodigoAndAvionAsync(string codigoAsiento, int avionId);
    Task SaveAsync(AircraftSeat seat);
    Task UpdateAsync(AircraftSeat seat);
    Task DeleteAsync(int id);
}