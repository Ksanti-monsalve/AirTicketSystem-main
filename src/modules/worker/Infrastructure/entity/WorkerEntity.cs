// src/modules/worker/Infrastructure/entity/WorkerEntity.cs
using AirTicketSystem.modules.airline.Infrastructure.entity;
using AirTicketSystem.modules.airport.Infrastructure.entity;
using AirTicketSystem.modules.checkin.Infrastructure.entity;
using AirTicketSystem.modules.flightcrew.Infrastructure.entity;
using AirTicketSystem.modules.person.Infrastructure.entity;
using AirTicketSystem.modules.pilotlicense.Infrastructure.entity;
using AirTicketSystem.modules.user.Infrastructure.entity;
using AirTicketSystem.modules.workertype.Infrastructure.entity;

namespace AirTicketSystem.modules.worker.Infrastructure.entity;

public class WorkerEntity
{
    public int Id { get; set; }
    public int PersonaId { get; set; }
    public int TipoTrabajadorId { get; set; }
    public int? AerolineaId { get; set; }
    public int AeropuertoBaseId { get; set; }
    public DateOnly FechaContratacion { get; set; }
    public decimal Salario { get; set; }
    public bool Activo { get; set; } = true;
    public int? UsuarioId { get; set; }

    public PersonEntity Persona { get; set; } = null!;
    public WorkerTypeEntity TipoTrabajador { get; set; } = null!;
    public AirlineEntity? Aerolinea { get; set; }
    public AirportEntity AeropuertoBase { get; set; } = null!;
    public UserEntity? Usuario { get; set; }
    public ICollection<WorkerSpecialtyEntity> Especialidades { get; set; } = new List<WorkerSpecialtyEntity>();
    public ICollection<PilotLicenseEntity> Licencias { get; set; } = new List<PilotLicenseEntity>();
    public ICollection<FlightCrewEntity> Tripulaciones { get; set; } = new List<FlightCrewEntity>();
    public ICollection<CheckInEntity> CheckInsAtendidos { get; set; } = new List<CheckInEntity>();
}