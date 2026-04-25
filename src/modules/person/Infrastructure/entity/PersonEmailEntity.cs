// src/modules/person/Infrastructure/entity/PersonEmailEntity.cs
using AirTicketSystem.modules.emailtype.Infrastructure.entity;

namespace AirTicketSystem.modules.person.Infrastructure.entity;

public class PersonEmailEntity
{
    public int Id { get; set; }
    public int PersonaId { get; set; }
    public int TipoEmailId { get; set; }
    public string Email { get; set; } = null!;
    public bool EsPrincipal { get; set; } = false;

    public PersonEntity Persona { get; set; } = null!;
    public EmailTypeEntity TipoEmail { get; set; } = null!;
}