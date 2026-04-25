// src/modules/worker/Domain/Repositories/IWorkerRepository.cs
using AirTicketSystem.modules.worker.Domain.aggregate;

namespace AirTicketSystem.modules.worker.Domain.Repositories;

public interface IWorkerRepository
{
    Task<Worker?> FindByIdAsync(int id);
    Task<IReadOnlyCollection<Worker>> FindAllAsync();
    Task<Worker?> FindByPersonaAsync(int personaId);
    Task<Worker?> FindByUsuarioAsync(int usuarioId);
    Task<IReadOnlyCollection<Worker>> FindByAerolineaAsync(int aerolineaId);
    Task<IReadOnlyCollection<Worker>> FindByAeropuertoBaseAsync(int aeropuertoId);
    Task<IReadOnlyCollection<Worker>> FindByTipoTrabajadorAsync(int tipoTrabajadorId);
    Task<IReadOnlyCollection<Worker>> FindActivosAsync();
    Task<IReadOnlyCollection<Worker>> FindPilotosHabilitadosParaModeloAsync(
        int modeloAvionId);
    Task SaveAsync(Worker worker);
    Task UpdateAsync(Worker worker);
    Task DeleteAsync(int id);
}