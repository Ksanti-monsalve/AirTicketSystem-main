// src/modules/contactrelationship/Application/Interfaces/IContactRelationshipService.cs
using AirTicketSystem.modules.contactrelationship.Domain.aggregate;

namespace AirTicketSystem.modules.contactrelationship.Application.Interfaces;

public interface IContactRelationshipService
{
    Task<ContactRelationship> CreateAsync(string descripcion);
    Task<ContactRelationship> GetByIdAsync(int id);
    Task<IReadOnlyCollection<ContactRelationship>> GetAllAsync();
    Task<ContactRelationship> UpdateAsync(int id, string descripcion);
    Task DeleteAsync(int id);
}
