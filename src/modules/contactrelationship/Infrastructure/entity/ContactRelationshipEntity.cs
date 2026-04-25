// src/modules/contactrelationship/Infrastructure/entity/ContactRelationshipEntity.cs
using AirTicketSystem.modules.client.Infrastructure.entity;

namespace AirTicketSystem.modules.contactrelationship.Infrastructure.entity;

public class ContactRelationshipEntity
{
    public int Id { get; set; }
    public string Descripcion { get; set; } = null!;
    public ICollection<EmergencyContactEntity> Contactos { get; set; } = new List<EmergencyContactEntity>();
}