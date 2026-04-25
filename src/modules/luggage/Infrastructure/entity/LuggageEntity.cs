// src/modules/luggage/Infrastructure/entity/LuggageEntity.cs
using AirTicketSystem.modules.bookingpassenger.Infrastructure.entity;
using AirTicketSystem.modules.flight.Infrastructure.entity;
using AirTicketSystem.modules.luggagetype.Infrastructure.entity;

namespace AirTicketSystem.modules.luggage.Infrastructure.entity;

public class LuggageEntity
{
    public int Id { get; set; }
    public int PasajeroReservaId { get; set; }
    public int VueloId { get; set; }
    public int TipoEquipajeId { get; set; }
    public string? Descripcion { get; set; }

    // Medidas declaradas al reservar
    public decimal? PesoDeclaradoKg { get; set; }
    public int? LargoDeclaradoCm { get; set; }
    public int? AnchoDeclaradoCm { get; set; }
    public int? AltoDeclaradoCm { get; set; }

    // Medidas reales registradas en check-in
    public decimal? PesoRealKg { get; set; }
    public int? LargoRealCm { get; set; }
    public int? AnchoRealCm { get; set; }
    public int? AltoRealCm { get; set; }

    public string? CodigoEquipaje { get; set; }
    public decimal CostoAdicional { get; set; } = 0;
    public string Estado { get; set; } = "DECLARADO";

    public BookingPassengerEntity PasajeroReserva { get; set; } = null!;
    public FlightEntity Vuelo { get; set; } = null!;
    public LuggageTypeEntity TipoEquipaje { get; set; } = null!;
}