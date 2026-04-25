// src/modules/phonetype/Infrastructure/entity/PhoneTypeEntity.cs
using AirTicketSystem.modules.airline.Infrastructure.entity;
using AirTicketSystem.modules.person.Infrastructure.entity;

namespace AirTicketSystem.modules.phonetype.Infrastructure.entity;

public class PhoneTypeEntity
{
    public int Id { get; set; }
    public string Descripcion { get; set; } = null!;
    public ICollection<PersonPhoneEntity> TelefonosPersona { get; set; } = new List<PersonPhoneEntity>();
    public ICollection<AirlinePhoneEntity> TelefonosAerolinea { get; set; } = new List<AirlinePhoneEntity>();
}