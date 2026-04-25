// src/modules/aircraft/Domain/Repositories/IAircraftRepository.cs
using AirTicketSystem.modules.aircraft.Domain.aggregate;

namespace AirTicketSystem.modules.aircraft.Domain.Repositories;

public interface IAircraftRepository
{
    Task<Aircraft?> FindByIdAsync(int id);
    Task<IReadOnlyCollection<Aircraft>> FindAllAsync();
    Task<Aircraft?> FindByMatriculaAsync(string matricula);
    Task<IReadOnlyCollection<Aircraft>> FindByAerolineaAsync(int aerolineaId);
    Task<IReadOnlyCollection<Aircraft>> FindByModeloAsync(int modeloAvionId);
    Task<IReadOnlyCollection<Aircraft>> FindByEstadoAsync(string estado);
    Task<IReadOnlyCollection<Aircraft>> FindDisponiblesAsync();
    Task<IReadOnlyCollection<Aircraft>> FindConMantenimientoUrgenteAsync();
    Task<bool> ExistsByMatriculaAsync(string matricula);
    Task SaveAsync(Aircraft aircraft);
    Task UpdateAsync(Aircraft aircraft);
    Task DeleteAsync(int id);
}