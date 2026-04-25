// src/modules/route/Infrastructure/entity/RouteEntity.cs
using AirTicketSystem.modules.airline.Infrastructure.entity;
using AirTicketSystem.modules.airport.Infrastructure.entity;
using AirTicketSystem.modules.fare.Infrastructure.entity;
using AirTicketSystem.modules.flight.Infrastructure.entity;

namespace AirTicketSystem.modules.route.Infrastructure.entity;

public class RouteEntity
{
    public int Id { get; set; }
    public int AerolineaId { get; set; }
    public int OrigenId { get; set; }
    public int DestinoId { get; set; }
    public int? DistanciaKm { get; set; }
    public int? DuracionEstimadaMin { get; set; }
    public bool Activa { get; set; } = true;

    public AirlineEntity Aerolinea { get; set; } = null!;
    public AirportEntity Origen { get; set; } = null!;
    public AirportEntity Destino { get; set; } = null!;
    public ICollection<FlightEntity> Vuelos { get; set; } = new List<FlightEntity>();
    public ICollection<FareEntity> Tarifas { get; set; } = new List<FareEntity>();
}