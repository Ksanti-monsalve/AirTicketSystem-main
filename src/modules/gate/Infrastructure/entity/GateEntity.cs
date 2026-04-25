// src/modules/gate/Infrastructure/entity/GateEntity.cs
using AirTicketSystem.modules.boardingpass.Infrastructure.entity;
using AirTicketSystem.modules.flight.Infrastructure.entity;
using AirTicketSystem.modules.terminal.Infrastructure.entity;

namespace AirTicketSystem.modules.gate.Infrastructure.entity;

public class GateEntity
{
    public int Id { get; set; }
    public int TerminalId { get; set; }
    public string Codigo { get; set; } = null!;
    public bool Activa { get; set; } = true;

    public TerminalEntity Terminal { get; set; } = null!;
    public ICollection<FlightEntity> Vuelos { get; set; } = new List<FlightEntity>();
    public ICollection<BoardingPassEntity> PasesAbordar { get; set; } = new List<BoardingPassEntity>();
}