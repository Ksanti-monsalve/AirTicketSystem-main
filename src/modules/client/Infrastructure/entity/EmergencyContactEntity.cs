// src/modules/client/Infrastructure/entity/EmergencyContactEntity.cs
using AirTicketSystem.modules.contactrelationship.Infrastructure.entity;
using AirTicketSystem.modules.person.Infrastructure.entity;

namespace AirTicketSystem.modules.client.Infrastructure.entity;

public class EmergencyContactEntity
{
    public int Id { get; set; }
    public int ClienteId { get; set; }
    public int PersonaId { get; set; }
    public int RelacionId { get; set; }
    public bool EsPrincipal { get; set; } = false;

    public ClientEntity Cliente { get; set; } = null!;
    public PersonEntity Persona { get; set; } = null!;
    public ContactRelationshipEntity Relacion { get; set; } = null!;
}