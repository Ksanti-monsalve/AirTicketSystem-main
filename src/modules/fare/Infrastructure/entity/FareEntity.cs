// src/modules/fare/Infrastructure/entity/FareEntity.cs
using AirTicketSystem.modules.booking.Infrastructure.entity;
using AirTicketSystem.modules.luggagerestriction.Infrastructure.entity;
using AirTicketSystem.modules.route.Infrastructure.entity;
using AirTicketSystem.modules.serviceclass.Infrastructure.entity;

namespace AirTicketSystem.modules.fare.Infrastructure.entity;

public class FareEntity
{
    public int Id { get; set; }
    public int RutaId { get; set; }
    public int ClaseServicioId { get; set; }
    public string Nombre { get; set; } = null!;
    public decimal PrecioBase { get; set; }
    public decimal Impuestos { get; set; } = 0;
    public decimal PrecioTotal { get; set; }
    public bool PermiteCambios { get; set; } = false;
    public bool PermiteReembolso { get; set; } = false;
    public bool Activa { get; set; } = true;
    public DateOnly? VigenteHasta { get; set; }

    public RouteEntity Ruta { get; set; } = null!;
    public ServiceClassEntity ClaseServicio { get; set; } = null!;
    public ICollection<LuggageRestrictionEntity> RestriccionesEquipaje { get; set; } = new List<LuggageRestrictionEntity>();
    public ICollection<BookingEntity> Reservas { get; set; } = new List<BookingEntity>();
}