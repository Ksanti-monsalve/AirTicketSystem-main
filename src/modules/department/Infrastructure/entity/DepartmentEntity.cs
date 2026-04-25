// src/modules/department/Infrastructure/entity/DepartmentEntity.cs
using AirTicketSystem.modules.city.Infrastructure.entity;
using AirTicketSystem.modules.region.Infrastructure.entity;

namespace AirTicketSystem.modules.department.Infrastructure.entity;

public class DepartmentEntity
{
    public int Id { get; set; }
    public int RegionId { get; set; }
    public string Nombre { get; set; } = null!;
    public string? Codigo { get; set; }

    public RegionEntity Region { get; set; } = null!;
    public ICollection<CityEntity> Ciudades { get; set; } = new List<CityEntity>();
}