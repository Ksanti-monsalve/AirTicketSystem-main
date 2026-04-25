// src/modules/airline/Domain/Repositories/IAirlinePhoneRepository.cs
using AirTicketSystem.modules.airline.Domain.aggregate;

namespace AirTicketSystem.modules.airline.Domain.Repositories;

public interface IAirlinePhoneRepository
{
    Task<AirlinePhone?> FindByIdAsync(int id);
    Task<IReadOnlyCollection<AirlinePhone>> FindByAerolineaAsync(int aerolineaId);
    Task<AirlinePhone?> FindPrincipalByAerolineaAsync(int aerolineaId);
    Task<bool> ExistsByNumeroAndAerolineaAsync(string numero, int aerolineaId);
    Task DesmarcarPrincipalByAerolineaAsync(int aerolineaId);
    Task SaveAsync(AirlinePhone phone);
    Task DeleteAsync(int id);
}
