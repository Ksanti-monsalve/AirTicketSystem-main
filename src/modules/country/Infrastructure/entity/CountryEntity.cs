// src/modules/country/Infrastructure/entity/CountryEntity.cs
using AirTicketSystem.modules.aircraftmanufacturer.Infrastructure.entity;
using AirTicketSystem.modules.airline.Infrastructure.entity;
using AirTicketSystem.modules.continent.Infrastructure.entity;
using AirTicketSystem.modules.person.Infrastructure.entity;
using AirTicketSystem.modules.region.Infrastructure.entity;

namespace AirTicketSystem.modules.country.Infrastructure.entity;

public class CountryEntity
{
    public int Id { get; set; }
    public int ContinenteId { get; set; }
    public string Nombre { get; set; } = null!;
    public string CodigoIso2 { get; set; } = null!;
    public string CodigoIso3 { get; set; } = null!;

    // Navegación
    public ContinentEntity Continente { get; set; } = null!;
    public ICollection<RegionEntity> Regiones { get; set; } = new List<RegionEntity>();
    public ICollection<AirlineEntity> Aerolineas { get; set; } = new List<AirlineEntity>();
    public ICollection<AircraftManufacturerEntity> Fabricantes { get; set; } = new List<AircraftManufacturerEntity>();
    public ICollection<PersonEntity> Personas { get; set; } = new List<PersonEntity>();
}

