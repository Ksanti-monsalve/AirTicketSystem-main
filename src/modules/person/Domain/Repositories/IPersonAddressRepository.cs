// src/modules/person/Domain/Repositories/IPersonAddressRepository.cs
using AirTicketSystem.modules.person.Domain.aggregate;

namespace AirTicketSystem.modules.person.Domain.Repositories;

public interface IPersonAddressRepository
{
    Task<PersonAddress?> FindByIdAsync(int id);
    Task<IReadOnlyCollection<PersonAddress>> FindByPersonaAsync(int personaId);
    Task<PersonAddress?> FindPrincipalByPersonaAsync(int personaId);
    Task<IReadOnlyCollection<PersonAddress>> FindByPersonaAndTipoAsync(
        int personaId, int tipoDireccionId);
    Task SaveAsync(PersonAddress address);
    Task UpdateAsync(PersonAddress address);
    Task DeleteAsync(int id);
    Task DesmarcarPrincipalByPersonaAsync(int personaId);
}
