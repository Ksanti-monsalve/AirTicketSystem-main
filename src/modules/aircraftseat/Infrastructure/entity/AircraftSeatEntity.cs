// src/modules/aircraftseat/Infrastructure/entity/AircraftSeatEntity.cs
using AirTicketSystem.modules.aircraft.Infrastructure.entity;
using AirTicketSystem.modules.seatavailability.Infrastructure.entity;
using AirTicketSystem.modules.serviceclass.Infrastructure.entity;
using AirTicketSystem.modules.ticket.Infrastructure.entity;

namespace AirTicketSystem.modules.aircraftseat.Infrastructure.entity;

public class AircraftSeatEntity
{
    public int Id { get; set; }
    public int AvionId { get; set; }
    public int ClaseServicioId { get; set; }
    public string CodigoAsiento { get; set; } = null!;
    public int Fila { get; set; }
    public string Columna { get; set; } = null!;
    public bool EsVentana { get; set; } = false;
    public bool EsPasillo { get; set; } = false;
    public bool Activo { get; set; } = true;
    public decimal CostoSeleccion { get; set; } = 0;

    public AircraftEntity Avion { get; set; } = null!;
    public ServiceClassEntity ClaseServicio { get; set; } = null!;
    public ICollection<SeatAvailabilityEntity> Disponibilidades { get; set; } = new List<SeatAvailabilityEntity>();
    public ICollection<TicketEntity> Tiquetes { get; set; } = new List<TicketEntity>();
}