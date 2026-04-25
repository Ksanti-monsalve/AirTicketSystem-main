// src/modules/city/Infrastructure/entity/CityEntity.cs
using AirTicketSystem.modules.airport.Infrastructure.entity;
using AirTicketSystem.modules.department.Infrastructure.entity;
using AirTicketSystem.modules.person.Infrastructure.entity;

namespace AirTicketSystem.modules.city.Infrastructure.entity;

public class CityEntity
{
    public int Id { get; set; }
    public int DepartamentoId { get; set; }
    public string Nombre { get; set; } = null!;
    public string? CodigoPostal { get; set; }

    public DepartmentEntity Departamento { get; set; } = null!;
    public ICollection<AirportEntity> Aeropuertos { get; set; } = new List<AirportEntity>();
    public ICollection<PersonAddressEntity> Direcciones { get; set; } = new List<PersonAddressEntity>();
}