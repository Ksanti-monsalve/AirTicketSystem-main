using System.Collections.Generic;
using AirTicketSystem.modules.country.Infrastructure.entity;

// src/modules/continent/Infrastructure/entity/ContinentEntity.cs
namespace AirTicketSystem.modules.continent.Infrastructure.entity;

public class ContinentEntity
{
    public int Id { get; set; }
    public string Nombre { get; set; } = null!;
    public string Codigo { get; set; } = null!;

    // Navegación
    public ICollection<CountryEntity> Paises { get; set; } = new List<CountryEntity>();
}