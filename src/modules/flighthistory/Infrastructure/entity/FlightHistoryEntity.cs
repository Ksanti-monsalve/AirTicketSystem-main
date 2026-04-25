// src/modules/flighthistory/Infrastructure/entity/FlightHistoryEntity.cs
using AirTicketSystem.modules.flight.Infrastructure.entity;
using AirTicketSystem.modules.user.Infrastructure.entity;

namespace AirTicketSystem.modules.flighthistory.Infrastructure.entity;

public class FlightHistoryEntity
{
    public int Id { get; set; }
    public int VueloId { get; set; }
    public string EstadoAnterior { get; set; } = null!;
    public string EstadoNuevo { get; set; } = null!;
    public DateTime FechaCambio { get; set; } = DateTime.UtcNow;
    public int? UsuarioId { get; set; }
    public string? Motivo { get; set; }

    public FlightEntity Vuelo { get; set; } = null!;
    public UserEntity? Usuario { get; set; }
}