// src/modules/person/Infrastructure/entity/PersonEntity.cs
using AirTicketSystem.modules.client.Infrastructure.entity;
using AirTicketSystem.modules.country.Infrastructure.entity;
using AirTicketSystem.modules.documenttype.Infrastructure.entity;
using AirTicketSystem.modules.gender.Infrastructure.entity;
using AirTicketSystem.modules.worker.Infrastructure.entity;

namespace AirTicketSystem.modules.person.Infrastructure.entity;

public class PersonEntity
{
    public int Id { get; set; }
    public int TipoDocId { get; set; }
    public string NumeroDoc { get; set; } = null!;
    public string Nombres { get; set; } = null!;
    public string Apellidos { get; set; } = null!;
    public DateOnly? FechaNacimiento { get; set; }
    public int? GeneroId { get; set; }
    public int? NacionalidadId { get; set; }

    public DocumentTypeEntity TipoDocumento { get; set; } = null!;
    public GenderEntity? Genero { get; set; }
    public CountryEntity? Nacionalidad { get; set; }
    public ICollection<PersonPhoneEntity> Telefonos { get; set; } = new List<PersonPhoneEntity>();
    public ICollection<PersonEmailEntity> Emails { get; set; } = new List<PersonEmailEntity>();
    public ICollection<PersonAddressEntity> Direcciones { get; set; } = new List<PersonAddressEntity>();
    public ClientEntity? Cliente { get; set; }
    public WorkerEntity? Trabajador { get; set; }
    public ICollection<EmergencyContactEntity> ContactosDeEmergencia { get; set; } = new List<EmergencyContactEntity>();
}