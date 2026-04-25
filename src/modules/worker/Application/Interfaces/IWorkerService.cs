// src/modules/worker/Application/Interfaces/IWorkerService.cs
using AirTicketSystem.modules.worker.Domain.aggregate;

namespace AirTicketSystem.modules.worker.Application.Interfaces;

public interface IWorkerService
{
    Task<Worker> CreateAsync(
        int personaId, int tipoTrabajadorId, int aeropuertoBaseId,
        DateOnly fechaContratacion, decimal salario,
        int? aerolineaId, int? usuarioId);
    Task<Worker> GetByIdAsync(int id);
    Task<Worker> GetByPersonAsync(int personaId);
    Task<IReadOnlyCollection<Worker>> GetAllAsync();
    Task<IReadOnlyCollection<Worker>> GetByAirlineAsync(int aerolineaId);
    Task<IReadOnlyCollection<Worker>> GetByAirportAsync(int aeropuertoId);
    Task<IReadOnlyCollection<Worker>> GetByWorkerTypeAsync(int tipoTrabajadorId);
    Task<IReadOnlyCollection<Worker>> GetActivosAsync();
    Task<IReadOnlyCollection<Worker>> GetQualifiedPilotsAsync(int modeloAvionId);
    Task<Worker> UpdateSalaryAsync(int id, decimal nuevoSalario);
    Task<Worker> UpdateAirportAsync(int id, int aeropuertoBaseId);
    Task<WorkerSpecialty> AssignSpecialtyAsync(int trabajadorId, int especialidadId);
    Task RemoveSpecialtyAsync(int workerSpecialtyId);
    Task<Worker> AssignUserAsync(int trabajadorId, int usuarioId);
    Task<Worker> ActivateAsync(int id);
    Task<Worker> DeactivateAsync(int id);
    Task DeleteAsync(int id);
}
