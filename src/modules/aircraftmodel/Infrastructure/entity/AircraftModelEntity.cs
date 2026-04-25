// src/modules/aircraftmodel/Infrastructure/entity/AircraftModelEntity.cs
using AirTicketSystem.modules.aircraft.Infrastructure.entity;
using AirTicketSystem.modules.aircraftmanufacturer.Infrastructure.entity;
using AirTicketSystem.modules.pilotrating.Infrastructure.entity;

namespace AirTicketSystem.modules.aircraftmodel.Infrastructure.entity;

public class AircraftModelEntity
{
    public int Id { get; set; }
    public int FabricanteId { get; set; }
    public string Nombre { get; set; } = null!;
    public string CodigoModelo { get; set; } = null!;
    public int? AutonomiKm { get; set; }
    public int? VelocidadCruceroKmh { get; set; }
    public string? Descripcion { get; set; }

    public AircraftManufacturerEntity Fabricante { get; set; } = null!;
    public ICollection<AircraftEntity> Aviones { get; set; } = new List<AircraftEntity>();
    public ICollection<PilotRatingEntity> Habilitaciones { get; set; } = new List<PilotRatingEntity>();
}