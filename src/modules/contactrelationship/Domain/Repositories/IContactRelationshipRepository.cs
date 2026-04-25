// src/modules/contactrelationship/Domain/Repositories/IContactRelationshipRepository.cs
using AirTicketSystem.modules.contactrelationship.Domain.aggregate;

namespace AirTicketSystem.modules.contactrelationship.Domain.Repositories;

public interface IContactRelationshipRepository
{
    Task<ContactRelationship?> FindByIdAsync(int id);
    Task<IReadOnlyCollection<ContactRelationship>> FindAllAsync();
    Task<bool> ExistsByDescripcionAsync(string descripcion);
    Task SaveAsync(ContactRelationship contactRelationship);
    Task UpdateAsync(ContactRelationship contactRelationship);
    Task DeleteAsync(int id);
}
