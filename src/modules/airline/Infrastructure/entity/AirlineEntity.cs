// src/modules/airline/Infrastructure/entity/AirlineEntity.cs
using AirTicketSystem.modules.aircraft.Infrastructure.entity;
using AirTicketSystem.modules.country.Infrastructure.entity;
using AirTicketSystem.modules.route.Infrastructure.entity;
using AirTicketSystem.modules.worker.Infrastructure.entity;

namespace AirTicketSystem.modules.airline.Infrastructure.entity;

public class AirlineEntity
{
    public int Id { get; set; }
    public string CodigoIata { get; set; } = null!;
    public string CodigoIcao { get; set; } = null!;
    public string Nombre { get; set; } = null!;
    public string? NombreComercial { get; set; }
    public int PaisId { get; set; }
    public string? SitioWeb { get; set; }
    public bool Activa { get; set; } = true;

    public CountryEntity Pais { get; set; } = null!;
    public ICollection<AirlinePhoneEntity> Telefonos { get; set; } = new List<AirlinePhoneEntity>();
    public ICollection<AirlineEmailEntity> Emails { get; set; } = new List<AirlineEmailEntity>();
    public ICollection<RouteEntity> Rutas { get; set; } = new List<RouteEntity>();
    public ICollection<AircraftEntity> Aviones { get; set; } = new List<AircraftEntity>();
    public ICollection<WorkerEntity> Trabajadores { get; set; } = new List<WorkerEntity>();
}