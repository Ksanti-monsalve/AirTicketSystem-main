// src/modules/boardingpass/Infrastructure/entity/BoardingPassEntity.cs
using AirTicketSystem.modules.checkin.Infrastructure.entity;
using AirTicketSystem.modules.gate.Infrastructure.entity;

namespace AirTicketSystem.modules.boardingpass.Infrastructure.entity;

public class BoardingPassEntity
{
    public int Id { get; set; }
    public int CheckinId { get; set; }
    public string CodigoPase { get; set; } = null!;
    public string? CodigoQr { get; set; }
    public int? PuertaEmbarqueId { get; set; }
    public DateTime? HoraEmbarque { get; set; }
    public DateTime FechaEmision { get; set; } = DateTime.UtcNow;

    public CheckInEntity Checkin { get; set; } = null!;
    public GateEntity? PuertaEmbarque { get; set; }
}