// src/modules/bookinghistory/Infrastructure/entity/BookingHistoryEntity.cs
using AirTicketSystem.modules.booking.Infrastructure.entity;
using AirTicketSystem.modules.user.Infrastructure.entity;

namespace AirTicketSystem.modules.bookinghistory.Infrastructure.entity;

public class BookingHistoryEntity
{
    public int Id { get; set; }
    public int ReservaId { get; set; }
    public string EstadoAnterior { get; set; } = null!;
    public string EstadoNuevo { get; set; } = null!;
    public DateTime FechaCambio { get; set; } = DateTime.UtcNow;
    public int? UsuarioId { get; set; }
    public string? Motivo { get; set; }

    public BookingEntity Reserva { get; set; } = null!;
    public UserEntity? Usuario { get; set; }
}