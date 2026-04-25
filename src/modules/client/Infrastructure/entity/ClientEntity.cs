// src/modules/client/Infrastructure/entity/ClientEntity.cs
using AirTicketSystem.modules.booking.Infrastructure.entity;
using AirTicketSystem.modules.person.Infrastructure.entity;
using AirTicketSystem.modules.user.Infrastructure.entity;

namespace AirTicketSystem.modules.client.Infrastructure.entity;

public class ClientEntity
{
    public int Id { get; set; }
    public int PersonaId { get; set; }
    public int UsuarioId { get; set; }
    public bool Activo { get; set; } = true;
    public DateTime FechaRegistro { get; set; } = DateTime.UtcNow;

    public PersonEntity Persona { get; set; } = null!;
    public UserEntity Usuario { get; set; } = null!;
    public ICollection<EmergencyContactEntity> ContactosEmergencia { get; set; } = new List<EmergencyContactEntity>();
    public ICollection<BookingEntity> Reservas { get; set; } = new List<BookingEntity>();
}