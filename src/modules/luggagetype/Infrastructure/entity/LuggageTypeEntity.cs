// src/modules/luggagetype/Infrastructure/entity/LuggageTypeEntity.cs
using AirTicketSystem.modules.luggage.Infrastructure.entity;
using AirTicketSystem.modules.luggagerestriction.Infrastructure.entity;

namespace AirTicketSystem.modules.luggagetype.Infrastructure.entity;

public class LuggageTypeEntity
{
    public int Id { get; set; }
    public string Nombre { get; set; } = null!;

    public ICollection<LuggageRestrictionEntity> Restricciones { get; set; } = new List<LuggageRestrictionEntity>();
    public ICollection<LuggageEntity> Equipajes { get; set; } = new List<LuggageEntity>();
}