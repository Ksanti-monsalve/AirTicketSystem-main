// src/modules/person/Infrastructure/entity/PersonPhoneEntity.cs
using AirTicketSystem.modules.phonetype.Infrastructure.entity;

namespace AirTicketSystem.modules.person.Infrastructure.entity;

public class PersonPhoneEntity
{
    public int Id { get; set; }
    public int PersonaId { get; set; }
    public int TipoTelefonoId { get; set; }
    public string Numero { get; set; } = null!;
    public string? IndicativoPais { get; set; }
    public bool EsPrincipal { get; set; } = false;

    public PersonEntity Persona { get; set; } = null!;
    public PhoneTypeEntity TipoTelefono { get; set; } = null!;
}