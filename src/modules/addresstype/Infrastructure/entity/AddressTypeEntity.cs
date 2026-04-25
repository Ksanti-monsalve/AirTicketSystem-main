// src/modules/addresstype/Infrastructure/entity/AddressTypeEntity.cs
using AirTicketSystem.modules.person.Infrastructure.entity;

namespace AirTicketSystem.modules.addresstype.Infrastructure.entity;

public class AddressTypeEntity
{
    public int Id { get; set; }
    public string Descripcion { get; set; } = null!;
    public ICollection<PersonAddressEntity> Direcciones { get; set; } = new List<PersonAddressEntity>();
}