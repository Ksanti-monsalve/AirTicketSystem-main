// src/modules/aircraft/Application/Services/AircraftService.cs
using AirTicketSystem.modules.aircraft.Application.Interfaces;
using AirTicketSystem.modules.aircraft.Application.UseCases;
using AirTicketSystem.modules.aircraft.Domain.aggregate;

namespace AirTicketSystem.modules.aircraft.Application.Services;

public class AircraftService : IAircraftService
{
    private readonly CreateAircraftUseCase _create;
    private readonly GetAircraftByIdUseCase _getById;
    private readonly GetAircraftByMatriculaUseCase _getByMatricula;
    private readonly GetAllAircraftUseCase _getAll;
    private readonly GetAircraftByAirlineUseCase _getByAirline;
    private readonly GetAvailableAircraftUseCase _getAvailable;
    private readonly GetAircraftWithUrgentMaintenanceUseCase _getUrgent;
    private readonly UpdateAircraftUseCase _update;
    private readonly SendToMaintenanceUseCase _sendToMaintenance;
    private readonly RegisterMaintenanceUseCase _registerMaintenance;
    private readonly RegisterLandingUseCase _registerLanding;
    private readonly DecommissionAircraftUseCase _decommission;
    private readonly ReactivateAircraftUseCase _reactivate;
    private readonly DeleteAircraftUseCase _delete;

    public AircraftService(
        CreateAircraftUseCase create,
        GetAircraftByIdUseCase getById,
        GetAircraftByMatriculaUseCase getByMatricula,
        GetAllAircraftUseCase getAll,
        GetAircraftByAirlineUseCase getByAirline,
        GetAvailableAircraftUseCase getAvailable,
        GetAircraftWithUrgentMaintenanceUseCase getUrgent,
        UpdateAircraftUseCase update,
        SendToMaintenanceUseCase sendToMaintenance,
        RegisterMaintenanceUseCase registerMaintenance,
        RegisterLandingUseCase registerLanding,
        DecommissionAircraftUseCase decommission,
        DeleteAircraftUseCase delete,
        ReactivateAircraftUseCase reactivate)
    {
        _create              = create;
        _getById             = getById;
        _getByMatricula      = getByMatricula;
        _getAll              = getAll;
        _getByAirline        = getByAirline;
        _getAvailable        = getAvailable;
        _getUrgent           = getUrgent;
        _update              = update;
        _sendToMaintenance   = sendToMaintenance;
        _registerMaintenance = registerMaintenance;
        _registerLanding     = registerLanding;
        _decommission        = decommission;
        _reactivate          = reactivate;
        _delete              = delete;
    }

    public Task<Aircraft> CreateAsync(
        int modeloAvionId, int aerolineaId, string matricula,
        DateOnly? fechaFabricacion, DateOnly? fechaProximoMantenimiento)
        => _create.ExecuteAsync(
            modeloAvionId, aerolineaId, matricula,
            fechaFabricacion, fechaProximoMantenimiento);

    public Task<Aircraft> GetByIdAsync(int id)
        => _getById.ExecuteAsync(id);

    public Task<Aircraft> GetByMatriculaAsync(string matricula)
        => _getByMatricula.ExecuteAsync(matricula);

    public Task<IReadOnlyCollection<Aircraft>> GetAllAsync()
        => _getAll.ExecuteAsync();

    public Task<IReadOnlyCollection<Aircraft>> GetByAirlineAsync(int aerolineaId)
        => _getByAirline.ExecuteAsync(aerolineaId);

    public Task<IReadOnlyCollection<Aircraft>> GetAvailableAsync()
        => _getAvailable.ExecuteAsync();

    public Task<IReadOnlyCollection<Aircraft>> GetWithUrgentMaintenanceAsync()
        => _getUrgent.ExecuteAsync();

    public Task<Aircraft> UpdateAsync(
        int id, DateOnly? fechaFabricacion,
        DateOnly? fechaProximoMantenimiento)
        => _update.ExecuteAsync(id, fechaFabricacion, fechaProximoMantenimiento);

    public Task SendToMaintenanceAsync(int id, DateOnly fechaProximoMantenimiento)
        => _sendToMaintenance.ExecuteAsync(id, fechaProximoMantenimiento);

    public Task RegisterMaintenanceAsync(int id, DateOnly fechaProximoMantenimiento)
        => _registerMaintenance.ExecuteAsync(id, fechaProximoMantenimiento);

    public Task RegisterLandingAsync(int id, decimal horasVuelo)
        => _registerLanding.ExecuteAsync(id, horasVuelo);

    public Task DecommissionAsync(int id)
        => _decommission.ExecuteAsync(id);

    public Task ReactivateAsync(int id)
        => _reactivate.ExecuteAsync(id);
    public Task DeleteAsync(int id) 
        => _delete.ExecuteAsync(id);
}