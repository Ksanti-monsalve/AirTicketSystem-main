// src/modules/documenttype/Infrastructure/entity/DocumentTypeEntity.cs
using AirTicketSystem.modules.person.Infrastructure.entity;

namespace AirTicketSystem.modules.documenttype.Infrastructure.entity;

public class DocumentTypeEntity
{
    public int Id { get; set; }
    public string Descripcion { get; set; } = null!;
    public ICollection<PersonEntity> Personas { get; set; } = new List<PersonEntity>();
}