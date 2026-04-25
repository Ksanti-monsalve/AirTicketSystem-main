// src/modules/airline/Infrastructure/entity/AirlinePhoneEntity.cs
using AirTicketSystem.modules.phonetype.Infrastructure.entity;

namespace AirTicketSystem.modules.airline.Infrastructure.entity;

public class AirlinePhoneEntity
{
    public int Id { get; set; }
    public int AerolineaId { get; set; }
    public int TipoTelefonoId { get; set; }
    public string Numero { get; set; } = null!;
    public string? IndicativoPais { get; set; }
    public bool EsPrincipal { get; set; } = false;

    public AirlineEntity Aerolinea { get; set; } = null!;
    public PhoneTypeEntity TipoTelefono { get; set; } = null!;
}