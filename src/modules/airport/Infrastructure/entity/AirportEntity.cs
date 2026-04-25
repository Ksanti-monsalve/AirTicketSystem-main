// src/modules/airport/Infrastructure/entity/AirportEntity.cs
using AirTicketSystem.modules.city.Infrastructure.entity;
using AirTicketSystem.modules.route.Infrastructure.entity;
using AirTicketSystem.modules.terminal.Infrastructure.entity;
using AirTicketSystem.modules.worker.Infrastructure.entity;

namespace AirTicketSystem.modules.airport.Infrastructure.entity;

public class AirportEntity
{
    public int Id { get; set; }
    public string CodigoIata { get; set; } = null!;
    public string CodigoIcao { get; set; } = null!;
    public string Nombre { get; set; } = null!;
    public int CiudadId { get; set; }
    public string? Direccion { get; set; }
    public bool Activo { get; set; } = true;

    public CityEntity Ciudad { get; set; } = null!;
    public ICollection<TerminalEntity> Terminales { get; set; } = new List<TerminalEntity>();
    public ICollection<RouteEntity> RutasOrigen { get; set; } = new List<RouteEntity>();
    public ICollection<RouteEntity> RutasDestino { get; set; } = new List<RouteEntity>();
    public ICollection<WorkerEntity> TrabajadoresBase { get; set; } = new List<WorkerEntity>();
}