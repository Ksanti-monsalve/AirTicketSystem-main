// src/modules/user/Infrastructure/entity/AccessLogEntity.cs
namespace AirTicketSystem.modules.user.Infrastructure.entity;

public class AccessLogEntity
{
    public int Id { get; set; }
    public int UsuarioId { get; set; }
    public DateTime FechaAcceso { get; set; } = DateTime.UtcNow;
    public string Tipo { get; set; } = null!;
    public string? IpAddress { get; set; }

    public UserEntity Usuario { get; set; } = null!;
}