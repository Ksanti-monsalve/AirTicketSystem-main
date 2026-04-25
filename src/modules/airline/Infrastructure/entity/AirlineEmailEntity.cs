// src/modules/airline/Infrastructure/entity/AirlineEmailEntity.cs
using AirTicketSystem.modules.emailtype.Infrastructure.entity;

namespace AirTicketSystem.modules.airline.Infrastructure.entity;

public class AirlineEmailEntity
{
    public int Id { get; set; }
    public int AerolineaId { get; set; }
    public int TipoEmailId { get; set; }
    public string Email { get; set; } = null!;
    public bool EsPrincipal { get; set; } = false;

    public AirlineEntity Aerolinea { get; set; } = null!;
    public EmailTypeEntity TipoEmail { get; set; } = null!;
}