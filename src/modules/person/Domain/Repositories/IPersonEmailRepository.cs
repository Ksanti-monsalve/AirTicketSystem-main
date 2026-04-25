// src/modules/person/Domain/Repositories/IPersonEmailRepository.cs
using AirTicketSystem.modules.person.Domain.aggregate;

namespace AirTicketSystem.modules.person.Domain.Repositories;

public interface IPersonEmailRepository
{
    Task<PersonEmail?> FindByIdAsync(int id);
    Task<IReadOnlyCollection<PersonEmail>> FindByPersonaAsync(int personaId);
    Task<PersonEmail?> FindPrincipalByPersonaAsync(int personaId);
    Task<PersonEmail?> FindByEmailAsync(string email);
    Task<bool> ExistsByEmailAsync(string email);
    Task SaveAsync(PersonEmail email);
    Task UpdateAsync(PersonEmail email);
    Task DeleteAsync(int id);
    Task DesmarcarPrincipalByPersonaAsync(int personaId);
}
