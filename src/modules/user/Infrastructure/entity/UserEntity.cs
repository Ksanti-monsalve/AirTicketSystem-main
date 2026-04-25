// src/modules/user/Infrastructure/entity/UserEntity.cs
using AirTicketSystem.modules.client.Infrastructure.entity;
using AirTicketSystem.modules.person.Infrastructure.entity;
using AirTicketSystem.modules.role.Infrastructure.entity;
using AirTicketSystem.modules.worker.Infrastructure.entity;

namespace AirTicketSystem.modules.user.Infrastructure.entity;

public class UserEntity
{
    public int Id { get; set; }
    public int PersonaId { get; set; }
    public int RolId { get; set; }
    public string Username { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public bool Activo { get; set; } = true;
    public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;
    public DateTime? UltimoLogin { get; set; }
    public int IntentosFallidos { get; set; } = 0;

    public PersonEntity Persona { get; set; } = null!;
    public RoleEntity Rol { get; set; } = null!;
    public ClientEntity? Cliente { get; set; }
    public WorkerEntity? Trabajador { get; set; }
    public ICollection<AccessLogEntity> LogsAcceso { get; set; } = new List<AccessLogEntity>();
}