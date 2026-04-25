// src/modules/luggagerestriction/Infrastructure/entity/LuggageRestrictionEntity.cs
using AirTicketSystem.modules.fare.Infrastructure.entity;
using AirTicketSystem.modules.luggagetype.Infrastructure.entity;

namespace AirTicketSystem.modules.luggagerestriction.Infrastructure.entity;

public class LuggageRestrictionEntity
{
    public int Id { get; set; }
    public int TarifaId { get; set; }
    public int TipoEquipajeId { get; set; }
    public int PiezasIncluidas { get; set; } = 0;
    public decimal PesoMaximoKg { get; set; }
    public int? LargoMaxCm { get; set; }
    public int? AnchoMaxCm { get; set; }
    public int? AltoMaxCm { get; set; }
    public decimal CostoExcesoKg { get; set; } = 0;

    public FareEntity Tarifa { get; set; } = null!;
    public LuggageTypeEntity TipoEquipaje { get; set; } = null!;
}