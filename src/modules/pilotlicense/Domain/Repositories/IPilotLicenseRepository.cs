// src/modules/pilotlicense/Domain/Repositories/IPilotLicenseRepository.cs
using AirTicketSystem.modules.pilotlicense.Domain.aggregate;

namespace AirTicketSystem.modules.pilotlicense.Domain.Repositories;

public interface IPilotLicenseRepository
{
    Task<PilotLicense?> FindByIdAsync(int id);
    Task<IReadOnlyCollection<PilotLicense>> FindAllAsync();
    Task<IReadOnlyCollection<PilotLicense>> FindByTrabajadorAsync(int trabajadorId);
    Task<IReadOnlyCollection<PilotLicense>> FindVigentesAsync();
    Task<IReadOnlyCollection<PilotLicense>> FindProximasAVencerAsync(int diasUmbral);
    Task<bool> ExistsByNumeroLicenciaAsync(string numeroLicencia);
    Task SaveAsync(PilotLicense license);
    Task UpdateAsync(PilotLicense license);
    Task DeleteAsync(int id);
}
