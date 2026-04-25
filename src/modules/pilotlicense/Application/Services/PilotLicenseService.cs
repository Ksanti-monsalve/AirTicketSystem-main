// src/modules/pilotlicense/Application/Services/PilotLicenseService.cs
using AirTicketSystem.modules.pilotlicense.Application.Interfaces;
using AirTicketSystem.modules.pilotlicense.Application.UseCases;
using AirTicketSystem.modules.pilotlicense.Domain.aggregate;

namespace AirTicketSystem.modules.pilotlicense.Application.Services;

public sealed class PilotLicenseService : IPilotLicenseService
{
    private readonly CreatePilotLicenseUseCase _create;
    private readonly GetPilotLicenseByIdUseCase _getById;
    private readonly GetLicensesByWorkerUseCase _getByWorker;
    private readonly GetVigenteLicensesUseCase _getVigentes;
    private readonly GetLicensesExpiringSoonUseCase _getExpiringSoon;
    private readonly RenewPilotLicenseUseCase _renew;
    private readonly SuspendPilotLicenseUseCase _suspend;
    private readonly ReactivatePilotLicenseUseCase _reactivate;
    private readonly DeletePilotLicenseUseCase _delete;

    public PilotLicenseService(
        CreatePilotLicenseUseCase create,
        GetPilotLicenseByIdUseCase getById,
        GetLicensesByWorkerUseCase getByWorker,
        GetVigenteLicensesUseCase getVigentes,
        GetLicensesExpiringSoonUseCase getExpiringSoon,
        RenewPilotLicenseUseCase renew,
        SuspendPilotLicenseUseCase suspend,
        ReactivatePilotLicenseUseCase reactivate,
        DeletePilotLicenseUseCase delete)
    {
        _create          = create;
        _getById         = getById;
        _getByWorker     = getByWorker;
        _getVigentes     = getVigentes;
        _getExpiringSoon = getExpiringSoon;
        _renew           = renew;
        _suspend         = suspend;
        _reactivate      = reactivate;
        _delete          = delete;
    }

    public Task<PilotLicense> CreateAsync(
        int trabajadorId, string numeroLicencia, string tipoLicencia,
        DateOnly fechaExpedicion, DateOnly fechaVencimiento,
        string autoridadEmisora)
        => _create.ExecuteAsync(
            trabajadorId, numeroLicencia, tipoLicencia,
            fechaExpedicion, fechaVencimiento, autoridadEmisora);

    public Task<PilotLicense> GetByIdAsync(int id)
        => _getById.ExecuteAsync(id);

    public Task<IReadOnlyCollection<PilotLicense>> GetByWorkerAsync(int trabajadorId)
        => _getByWorker.ExecuteAsync(trabajadorId);

    public Task<IReadOnlyCollection<PilotLicense>> GetVigentesAsync()
        => _getVigentes.ExecuteAsync();

    public Task<IReadOnlyCollection<PilotLicense>> GetProximasAVencerAsync(int diasUmbral)
        => _getExpiringSoon.ExecuteAsync(diasUmbral);

    public Task<PilotLicense> RenewAsync(int id, DateOnly nuevaFechaVencimiento)
        => _renew.ExecuteAsync(id, nuevaFechaVencimiento);

    public Task SuspendAsync(int id)    => _suspend.ExecuteAsync(id);
    public Task ReactivateAsync(int id) => _reactivate.ExecuteAsync(id);
    public Task DeleteAsync(int id)     => _delete.ExecuteAsync(id);
}
