// src/modules/pilotlicense/Application/Interfaces/IPilotLicenseService.cs
using AirTicketSystem.modules.pilotlicense.Domain.aggregate;

namespace AirTicketSystem.modules.pilotlicense.Application.Interfaces;

public interface IPilotLicenseService
{
    Task<PilotLicense> CreateAsync(
        int trabajadorId, string numeroLicencia, string tipoLicencia,
        DateOnly fechaExpedicion, DateOnly fechaVencimiento,
        string autoridadEmisora);
    Task<PilotLicense> GetByIdAsync(int id);
    Task<IReadOnlyCollection<PilotLicense>> GetByWorkerAsync(int trabajadorId);
    Task<IReadOnlyCollection<PilotLicense>> GetVigentesAsync();
    Task<IReadOnlyCollection<PilotLicense>> GetProximasAVencerAsync(int diasUmbral);
    Task<PilotLicense> RenewAsync(int id, DateOnly nuevaFechaVencimiento);
    Task SuspendAsync(int id);
    Task ReactivateAsync(int id);
    Task DeleteAsync(int id);
}
