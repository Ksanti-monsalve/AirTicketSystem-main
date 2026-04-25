// src/modules/flight/Infrastructure/entity/FlightEntity.cs
using AirTicketSystem.modules.aircraft.Infrastructure.entity;
using AirTicketSystem.modules.booking.Infrastructure.entity;
using AirTicketSystem.modules.flightcrew.Infrastructure.entity;
using AirTicketSystem.modules.flighthistory.Infrastructure.entity;
using AirTicketSystem.modules.gate.Infrastructure.entity;
using AirTicketSystem.modules.luggage.Infrastructure.entity;
using AirTicketSystem.modules.route.Infrastructure.entity;
using AirTicketSystem.modules.seatavailability.Infrastructure.entity;

namespace AirTicketSystem.modules.flight.Infrastructure.entity;

public class FlightEntity
{
    public int Id { get; set; }
    public string NumeroVuelo { get; set; } = null!;
    public int RutaId { get; set; }
    public int AvionId { get; set; }
    public int? PuertaEmbarqueId { get; set; }
    public DateTime FechaSalida { get; set; }
    public DateTime FechaLlegadaEstimada { get; set; }
    public DateTime? FechaLlegadaReal { get; set; }
    public string Estado { get; set; } = "PROGRAMADO";
    public string? MotivoCambioEstado { get; set; }
    public DateTime? CheckinApertura { get; set; }
    public DateTime? CheckinCierre { get; set; }

    public RouteEntity Ruta { get; set; } = null!;
    public AircraftEntity Avion { get; set; } = null!;
    public GateEntity? PuertaEmbarque { get; set; }
    public ICollection<FlightCrewEntity> Tripulacion { get; set; } = new List<FlightCrewEntity>();
    public ICollection<SeatAvailabilityEntity> DisponibilidadAsientos { get; set; } = new List<SeatAvailabilityEntity>();
    public ICollection<FlightHistoryEntity> Historial { get; set; } = new List<FlightHistoryEntity>();
    public ICollection<BookingEntity> Reservas { get; set; } = new List<BookingEntity>();
    public ICollection<LuggageEntity> Equipajes { get; set; } = new List<LuggageEntity>();
}