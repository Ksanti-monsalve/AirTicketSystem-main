// src/modules/airline/Domain/Repositories/IAirlineEmailRepository.cs
using AirTicketSystem.modules.airline.Domain.aggregate;

namespace AirTicketSystem.modules.airline.Domain.Repositories;

public interface IAirlineEmailRepository
{
    Task<AirlineEmail?> FindByIdAsync(int id);
    Task<IReadOnlyCollection<AirlineEmail>> FindByAerolineaAsync(int aerolineaId);
    Task<AirlineEmail?> FindPrincipalByAerolineaAsync(int aerolineaId);
    Task<bool> ExistsByEmailAndAerolineaAsync(string email, int aerolineaId);
    Task DesmarcarPrincipalByAerolineaAsync(int aerolineaId);
    Task SaveAsync(AirlineEmail email);
    Task DeleteAsync(int id);
}
