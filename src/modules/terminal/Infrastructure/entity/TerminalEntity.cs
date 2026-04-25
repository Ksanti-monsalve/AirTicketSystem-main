// src/modules/terminal/Infrastructure/entity/TerminalEntity.cs
using AirTicketSystem.modules.airport.Infrastructure.entity;
using AirTicketSystem.modules.gate.Infrastructure.entity;

namespace AirTicketSystem.modules.terminal.Infrastructure.entity;

public class TerminalEntity
{
    public int Id { get; set; }
    public int AeropuertoId { get; set; }
    public string Nombre { get; set; } = null!;
    public string? Descripcion { get; set; }

    public AirportEntity Aeropuerto { get; set; } = null!;
    public ICollection<GateEntity> Puertas { get; set; } = new List<GateEntity>();
}