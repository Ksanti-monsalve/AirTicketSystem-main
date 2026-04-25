// src/modules/emailtype/Infrastructure/entity/EmailTypeEntity.cs
using AirTicketSystem.modules.airline.Infrastructure.entity;
using AirTicketSystem.modules.person.Infrastructure.entity;

namespace AirTicketSystem.modules.emailtype.Infrastructure.entity;

public class EmailTypeEntity
{
    public int Id { get; set; }
    public string Descripcion { get; set; } = null!;
    public ICollection<PersonEmailEntity> EmailsPersona { get; set; } = new List<PersonEmailEntity>();
    public ICollection<AirlineEmailEntity> EmailsAerolinea { get; set; } = new List<AirlineEmailEntity>();
}