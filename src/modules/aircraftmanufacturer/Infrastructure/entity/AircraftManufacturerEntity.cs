// src/modules/aircraftmanufacturer/Infrastructure/entity/AircraftManufacturerEntity.cs
using AirTicketSystem.modules.aircraftmodel.Infrastructure.entity;
using AirTicketSystem.modules.country.Infrastructure.entity;

namespace AirTicketSystem.modules.aircraftmanufacturer.Infrastructure.entity;

public class AircraftManufacturerEntity
{
    public int Id { get; set; }
    public string Nombre { get; set; } = null!;
    public int PaisId { get; set; }
    public string? SitioWeb { get; set; }

    public CountryEntity Pais { get; set; } = null!;
    public ICollection<AircraftModelEntity> Modelos { get; set; } = new List<AircraftModelEntity>();
}