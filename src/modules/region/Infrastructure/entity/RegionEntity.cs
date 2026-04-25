
// src/modules/region/Infrastructure/entity/RegionEntity.cs
using AirTicketSystem.modules.country.Infrastructure.entity;
using AirTicketSystem.modules.department.Infrastructure.entity;

namespace AirTicketSystem.modules.region.Infrastructure.entity;

public class RegionEntity
{
    public int Id { get; set; }
    public int PaisId { get; set; }
    public string Nombre { get; set; } = null!;
    public string? Codigo { get; set; }

    public CountryEntity Pais { get; set; } = null!;
    public ICollection<DepartmentEntity> Departamentos { get; set; } = new List<DepartmentEntity>();
}