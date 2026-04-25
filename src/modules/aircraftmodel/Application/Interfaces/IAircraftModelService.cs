// src/modules/aircraftmodel/Application/Interfaces/IAircraftModelService.cs
using AirTicketSystem.modules.aircraftmodel.Domain.aggregate;

namespace AirTicketSystem.modules.aircraftmodel.Application.Interfaces;

public interface IAircraftModelService
{
    Task<AircraftModel> CreateAsync(
        int fabricanteId, string nombre, string codigoModelo,
        int? autonomiKm, int? velocidadKmh, string? descripcion);
    Task<AircraftModel> GetByIdAsync(int id);
    Task<IReadOnlyCollection<AircraftModel>> GetAllAsync();
    Task<IReadOnlyCollection<AircraftModel>> GetByManufacturerAsync(int fabricanteId);
    Task<AircraftModel> UpdateAsync(
        int id, string nombre, int? autonomiKm,
        int? velocidadKmh, string? descripcion);
    Task DeleteAsync(int id);
}