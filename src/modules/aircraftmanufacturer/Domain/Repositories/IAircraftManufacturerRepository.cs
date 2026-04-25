// src/modules/aircraftmanufacturer/Domain/Repositories/IAircraftManufacturerRepository.cs
using AirTicketSystem.modules.aircraftmanufacturer.Domain.aggregate;

namespace AirTicketSystem.modules.aircraftmanufacturer.Domain.Repositories;

public interface IAircraftManufacturerRepository
{
    Task<AircraftManufacturer?> FindByIdAsync(int id);
    Task<IReadOnlyCollection<AircraftManufacturer>> FindAllAsync();
    Task<AircraftManufacturer?> FindByNombreAsync(string nombre);
    Task<IReadOnlyCollection<AircraftManufacturer>> FindByPaisAsync(int paisId);
    Task<bool> ExistsByNombreAsync(string nombre);
    Task SaveAsync(AircraftManufacturer manufacturer);
    Task UpdateAsync(AircraftManufacturer manufacturer);
    Task DeleteAsync(int id);
}