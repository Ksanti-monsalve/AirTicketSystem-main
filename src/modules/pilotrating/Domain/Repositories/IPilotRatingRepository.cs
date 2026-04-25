// src/modules/pilotrating/Domain/Repositories/IPilotRatingRepository.cs
using AirTicketSystem.modules.pilotrating.Domain.aggregate;

namespace AirTicketSystem.modules.pilotrating.Domain.Repositories;

public interface IPilotRatingRepository
{
    Task<PilotRating?> FindByIdAsync(int id);
    Task<IReadOnlyCollection<PilotRating>> FindAllAsync();
    Task<IReadOnlyCollection<PilotRating>> FindByLicenciaAsync(int licenciaId);
    Task<IReadOnlyCollection<PilotRating>> FindByModeloAvionAsync(int modeloAvionId);
    Task<PilotRating?> FindByLicenciaAndModeloAsync(int licenciaId, int modeloAvionId);
    Task<bool> ExistsByLicenciaAndModeloAsync(int licenciaId, int modeloAvionId);
    Task<IReadOnlyCollection<PilotRating>> FindVigentesAsync();
    Task SaveAsync(PilotRating rating);
    Task UpdateAsync(PilotRating rating);
    Task DeleteAsync(int id);
}
