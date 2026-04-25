// src/modules/person/Infrastructure/entity/PersonAddressEntity.cs
using AirTicketSystem.modules.addresstype.Infrastructure.entity;
using AirTicketSystem.modules.city.Infrastructure.entity;
using AirTicketSystem.modules.invoice.Infrastructure.entity;

namespace AirTicketSystem.modules.person.Infrastructure.entity;

public class PersonAddressEntity
{
    public int Id { get; set; }
    public int PersonaId { get; set; }
    public int TipoDireccionId { get; set; }
    public int CiudadId { get; set; }
    public string DireccionLinea1 { get; set; } = null!;
    public string? DireccionLinea2 { get; set; }
    public string? CodigoPostal { get; set; }
    public bool EsPrincipal { get; set; } = false;

    public PersonEntity Persona { get; set; } = null!;
    public AddressTypeEntity TipoDireccion { get; set; } = null!;
    public CityEntity Ciudad { get; set; } = null!;
    public ICollection<InvoiceEntity> Facturas { get; set; } = new List<InvoiceEntity>();
}