// src/modules/aircraftmanufacturer/Application/Interfaces/IAircraftManufacturerService.cs
using AirTicketSystem.modules.aircraftmanufacturer.Domain.aggregate;

namespace AirTicketSystem.modules.aircraftmanufacturer.Application.Interfaces;

public interface IAircraftManufacturerService
{
    Task<AircraftManufacturer> CreateAsync(
        int paisId, string nombre, string? sitioWeb);
    Task<AircraftManufacturer> GetByIdAsync(int id);
    Task<IReadOnlyCollection<AircraftManufacturer>> GetAllAsync();
    Task<IReadOnlyCollection<AircraftManufacturer>> GetByCountryAsync(int paisId);
    Task<AircraftManufacturer> UpdateAsync(
        int id, string nombre, string? sitioWeb);
    Task DeleteAsync(int id);
}