// src/modules/pilotrating/Application/Interfaces/IPilotRatingService.cs
using AirTicketSystem.modules.pilotrating.Domain.aggregate;

namespace AirTicketSystem.modules.pilotrating.Application.Interfaces;

public interface IPilotRatingService
{
    Task<PilotRating> CreateAsync(
        int licenciaId, int modeloAvionId,
        DateOnly fechaHabilitacion, DateOnly fechaVencimiento);
    Task<PilotRating> GetByIdAsync(int id);
    Task<IReadOnlyCollection<PilotRating>> GetByLicenseAsync(int licenciaId);
    Task<IReadOnlyCollection<PilotRating>> GetByAircraftModelAsync(int modeloAvionId);
    Task<IReadOnlyCollection<PilotRating>> GetVigentesAsync();
    Task<PilotRating> RenewAsync(int id, DateOnly nuevaFechaVencimiento);
    Task DeleteAsync(int id);
}
