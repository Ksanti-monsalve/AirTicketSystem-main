// src/modules/worker/Application/Services/WorkerService.cs
using AirTicketSystem.modules.worker.Domain.aggregate;
using AirTicketSystem.modules.worker.Application.Interfaces;
using AirTicketSystem.modules.worker.Application.UseCases;

namespace AirTicketSystem.modules.worker.Application.Services;

public sealed class WorkerService : IWorkerService
{
    private readonly CreateWorkerUseCase           _create;
    private readonly GetWorkerByIdUseCase          _getById;
    private readonly GetWorkerByPersonUseCase      _getByPerson;
    private readonly GetAllWorkersUseCase          _getAll;
    private readonly GetWorkersByAirlineUseCase    _getByAirline;
    private readonly GetWorkersByAirportUseCase    _getByAirport;
    private readonly GetWorkersByWorkerTypeUseCase _getByWorkerType;
    private readonly GetActiveWorkersUseCase       _getActivos;
    private readonly GetQualifiedPilotsUseCase     _getQualifiedPilots;
    private readonly UpdateWorkerSalaryUseCase     _updateSalary;
    private readonly UpdateWorkerAirportUseCase    _updateAirport;
    private readonly AssignWorkerSpecialtyUseCase  _assignSpecialty;
    private readonly RemoveWorkerSpecialtyUseCase  _removeSpecialty;
    private readonly AssignUserToWorkerUseCase     _assignUser;
    private readonly ActivateWorkerUseCase         _activate;
    private readonly DeactivateWorkerUseCase       _deactivate;
    private readonly DeleteWorkerUseCase           _delete;

    public WorkerService(
        CreateWorkerUseCase create,
        GetWorkerByIdUseCase getById,
        GetWorkerByPersonUseCase getByPerson,
        GetAllWorkersUseCase getAll,
        GetWorkersByAirlineUseCase getByAirline,
        GetWorkersByAirportUseCase getByAirport,
        GetWorkersByWorkerTypeUseCase getByWorkerType,
        GetActiveWorkersUseCase getActivos,
        GetQualifiedPilotsUseCase getQualifiedPilots,
        UpdateWorkerSalaryUseCase updateSalary,
        UpdateWorkerAirportUseCase updateAirport,
        AssignWorkerSpecialtyUseCase assignSpecialty,
        RemoveWorkerSpecialtyUseCase removeSpecialty,
        AssignUserToWorkerUseCase assignUser,
        ActivateWorkerUseCase activate,
        DeactivateWorkerUseCase deactivate,
        DeleteWorkerUseCase delete)
    {
        _create             = create;
        _getById            = getById;
        _getByPerson        = getByPerson;
        _getAll             = getAll;
        _getByAirline       = getByAirline;
        _getByAirport       = getByAirport;
        _getByWorkerType    = getByWorkerType;
        _getActivos         = getActivos;
        _getQualifiedPilots = getQualifiedPilots;
        _updateSalary       = updateSalary;
        _updateAirport      = updateAirport;
        _assignSpecialty    = assignSpecialty;
        _removeSpecialty    = removeSpecialty;
        _assignUser         = assignUser;
        _activate           = activate;
        _deactivate         = deactivate;
        _delete             = delete;
    }

    public Task<Worker> CreateAsync(
        int personaId, int tipoTrabajadorId, int aeropuertoBaseId,
        DateOnly fechaContratacion, decimal salario,
        int? aerolineaId, int? usuarioId)
        => _create.ExecuteAsync(personaId, tipoTrabajadorId, aeropuertoBaseId,
            fechaContratacion, salario, aerolineaId, usuarioId);

    public Task<Worker> GetByIdAsync(int id)
        => _getById.ExecuteAsync(id);

    public Task<Worker> GetByPersonAsync(int personaId)
        => _getByPerson.ExecuteAsync(personaId);

    public Task<IReadOnlyCollection<Worker>> GetAllAsync()
        => _getAll.ExecuteAsync();

    public Task<IReadOnlyCollection<Worker>> GetByAirlineAsync(int aerolineaId)
        => _getByAirline.ExecuteAsync(aerolineaId);

    public Task<IReadOnlyCollection<Worker>> GetByAirportAsync(int aeropuertoId)
        => _getByAirport.ExecuteAsync(aeropuertoId);

    public Task<IReadOnlyCollection<Worker>> GetByWorkerTypeAsync(int tipoTrabajadorId)
        => _getByWorkerType.ExecuteAsync(tipoTrabajadorId);

    public Task<IReadOnlyCollection<Worker>> GetActivosAsync()
        => _getActivos.ExecuteAsync();

    public Task<IReadOnlyCollection<Worker>> GetQualifiedPilotsAsync(int modeloAvionId)
        => _getQualifiedPilots.ExecuteAsync(modeloAvionId);

    public Task<Worker> UpdateSalaryAsync(int id, decimal nuevoSalario)
        => _updateSalary.ExecuteAsync(id, nuevoSalario);

    public Task<Worker> UpdateAirportAsync(int id, int aeropuertoBaseId)
        => _updateAirport.ExecuteAsync(id, aeropuertoBaseId);

    public Task<WorkerSpecialty> AssignSpecialtyAsync(int trabajadorId, int especialidadId)
        => _assignSpecialty.ExecuteAsync(trabajadorId, especialidadId);

    public Task RemoveSpecialtyAsync(int workerSpecialtyId)
        => _removeSpecialty.ExecuteAsync(workerSpecialtyId);

    public Task<Worker> AssignUserAsync(int trabajadorId, int usuarioId)
        => _assignUser.ExecuteAsync(trabajadorId, usuarioId);

    public Task<Worker> ActivateAsync(int id)
        => _activate.ExecuteAsync(id);

    public Task<Worker> DeactivateAsync(int id)
        => _deactivate.ExecuteAsync(id);

    public Task DeleteAsync(int id)
        => _delete.ExecuteAsync(id);
}
