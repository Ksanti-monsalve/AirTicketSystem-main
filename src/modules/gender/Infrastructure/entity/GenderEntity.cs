// src/modules/gender/Infrastructure/entity/GenderEntity.cs
using AirTicketSystem.modules.person.Infrastructure.entity;

namespace AirTicketSystem.modules.gender.Infrastructure.entity;

public class GenderEntity
{
    public int Id { get; set; }
    public string Nombre { get; set; } = null!;
    public ICollection<PersonEntity> Personas { get; set; } = new List<PersonEntity>();
}