// src/modules/person/Domain/Repositories/IPersonPhoneRepository.cs
using AirTicketSystem.modules.person.Domain.aggregate;

namespace AirTicketSystem.modules.person.Domain.Repositories;

public interface IPersonPhoneRepository
{
    Task<PersonPhone?> FindByIdAsync(int id);
    Task<IReadOnlyCollection<PersonPhone>> FindByPersonaAsync(int personaId);
    Task<PersonPhone?> FindPrincipalByPersonaAsync(int personaId);
    Task<bool> ExistsByNumeroAndPersonaAsync(string numero, int personaId);
    Task SaveAsync(PersonPhone phone);
    Task UpdateAsync(PersonPhone phone);
    Task DeleteAsync(int id);
    Task DesmarcarPrincipalByPersonaAsync(int personaId);
}
