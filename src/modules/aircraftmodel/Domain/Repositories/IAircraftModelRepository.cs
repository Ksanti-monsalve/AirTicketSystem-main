// src/modules/aircraftmodel/Domain/Repositories/IAircraftModelRepository.cs
using AirTicketSystem.modules.aircraftmodel.Domain.aggregate;

namespace AirTicketSystem.modules.aircraftmodel.Domain.Repositories;

public interface IAircraftModelRepository
{
    Task<AircraftModel?> FindByIdAsync(int id);
    Task<IReadOnlyCollection<AircraftModel>> FindAllAsync();
    Task<AircraftModel?> FindByCodigoModeloAsync(string codigoModelo);
    Task<IReadOnlyCollection<AircraftModel>> FindByFabricanteAsync(int fabricanteId);
    Task<bool> ExistsByCodigoModeloAsync(string codigoModelo);
    Task SaveAsync(AircraftModel model);
    Task UpdateAsync(AircraftModel model);
    Task DeleteAsync(int id);
}