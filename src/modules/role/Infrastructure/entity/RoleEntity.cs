// src/modules/role/Infrastructure/entity/RoleEntity.cs
using AirTicketSystem.modules.user.Infrastructure.entity;

namespace AirTicketSystem.modules.role.Infrastructure.entity;

public class RoleEntity
{
    public int Id { get; set; }
    public string Nombre { get; set; } = null!;
    public ICollection<UserEntity> Usuarios { get; set; } = new List<UserEntity>();
}