// src/modules/aircraft/Application/Interfaces/IAircraftService.cs
using AirTicketSystem.modules.aircraft.Domain.aggregate;

namespace AirTicketSystem.modules.aircraft.Application.Interfaces;

public interface IAircraftService
{
    Task<Aircraft> CreateAsync(
        int modeloAvionId, int aerolineaId, string matricula,
        DateOnly? fechaFabricacion, DateOnly? fechaProximoMantenimiento);
    Task<Aircraft> GetByIdAsync(int id);
    Task<Aircraft> GetByMatriculaAsync(string matricula);
    Task<IReadOnlyCollection<Aircraft>> GetAllAsync();
    Task<IReadOnlyCollection<Aircraft>> GetByAirlineAsync(int aerolineaId);
    Task<IReadOnlyCollection<Aircraft>> GetAvailableAsync();
    Task<IReadOnlyCollection<Aircraft>> GetWithUrgentMaintenanceAsync();
    Task<Aircraft> UpdateAsync(
        int id, DateOnly? fechaFabricacion,
        DateOnly? fechaProximoMantenimiento);
    Task SendToMaintenanceAsync(int id, DateOnly fechaProximoMantenimiento);
    Task RegisterMaintenanceAsync(int id, DateOnly fechaProximoMantenimiento);
    Task RegisterLandingAsync(int id, decimal horasVuelo);
    Task DecommissionAsync(int id);
    Task ReactivateAsync(int id);
    Task DeleteAsync(int id);
}