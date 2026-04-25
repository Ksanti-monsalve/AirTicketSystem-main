// src/modules/person/Domain/Repositories/IPersonRepository.cs
using AirTicketSystem.modules.person.Domain.aggregate;

namespace AirTicketSystem.modules.person.Domain.Repositories;

public interface IPersonRepository
{
    Task<Person?> FindByIdAsync(int id);
    Task<IReadOnlyCollection<Person>> FindAllAsync();
    Task<Person?> FindByDocumentoAsync(int tipoDocId, string numeroDoc);
    Task<bool> ExistsByDocumentoAsync(int tipoDocId, string numeroDoc);
    Task SaveAsync(Person person);
    Task UpdateAsync(Person person);
    Task DeleteAsync(int id);
}
