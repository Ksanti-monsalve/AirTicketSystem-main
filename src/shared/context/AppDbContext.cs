// src/shared/context/AppDbContext.cs
using Microsoft.EntityFrameworkCore;

// Geografía
using AirTicketSystem.modules.continent.Infrastructure.entity;
using AirTicketSystem.modules.country.Infrastructure.entity;
using AirTicketSystem.modules.region.Infrastructure.entity;
using AirTicketSystem.modules.department.Infrastructure.entity;
using AirTicketSystem.modules.city.Infrastructure.entity;
using AirTicketSystem.modules.airport.Infrastructure.entity;
using AirTicketSystem.modules.terminal.Infrastructure.entity;
using AirTicketSystem.modules.gate.Infrastructure.entity;

// Catálogos
using AirTicketSystem.modules.gender.Infrastructure.entity;
using AirTicketSystem.modules.documenttype.Infrastructure.entity;
using AirTicketSystem.modules.phonetype.Infrastructure.entity;
using AirTicketSystem.modules.emailtype.Infrastructure.entity;
using AirTicketSystem.modules.addresstype.Infrastructure.entity;
using AirTicketSystem.modules.contactrelationship.Infrastructure.entity;

// Personas y usuarios
using AirTicketSystem.modules.person.Infrastructure.entity;
using AirTicketSystem.modules.role.Infrastructure.entity;
using AirTicketSystem.modules.user.Infrastructure.entity;
using AirTicketSystem.modules.client.Infrastructure.entity;

// Aerolíneas
using AirTicketSystem.modules.airline.Infrastructure.entity;
using AirTicketSystem.modules.route.Infrastructure.entity;

// Flota
using AirTicketSystem.modules.aircraftmanufacturer.Infrastructure.entity;
using AirTicketSystem.modules.aircraftmodel.Infrastructure.entity;
using AirTicketSystem.modules.aircraft.Infrastructure.entity;
using AirTicketSystem.modules.serviceclass.Infrastructure.entity;
using AirTicketSystem.modules.aircraftseat.Infrastructure.entity;

// Personal
using AirTicketSystem.modules.workertype.Infrastructure.entity;
using AirTicketSystem.modules.specialty.Infrastructure.entity;
using AirTicketSystem.modules.worker.Infrastructure.entity;
using AirTicketSystem.modules.pilotlicense.Infrastructure.entity;
using AirTicketSystem.modules.pilotrating.Infrastructure.entity;

// Vuelos
using AirTicketSystem.modules.flight.Infrastructure.entity;
using AirTicketSystem.modules.flightcrew.Infrastructure.entity;
using AirTicketSystem.modules.seatavailability.Infrastructure.entity;
using AirTicketSystem.modules.flighthistory.Infrastructure.entity;

// Tarifas y equipaje
using AirTicketSystem.modules.fare.Infrastructure.entity;
using AirTicketSystem.modules.luggagetype.Infrastructure.entity;
using AirTicketSystem.modules.luggagerestriction.Infrastructure.entity;

// Reservas y tiquetes
using AirTicketSystem.modules.booking.Infrastructure.entity;
using AirTicketSystem.modules.bookingpassenger.Infrastructure.entity;
using AirTicketSystem.modules.bookinghistory.Infrastructure.entity;
using AirTicketSystem.modules.ticket.Infrastructure.entity;
using AirTicketSystem.modules.luggage.Infrastructure.entity;

// Check-in y pagos
using AirTicketSystem.modules.checkin.Infrastructure.entity;
using AirTicketSystem.modules.boardingpass.Infrastructure.entity;
using AirTicketSystem.modules.paymentmethod.Infrastructure.entity;
using AirTicketSystem.modules.payment.Infrastructure.entity;
using AirTicketSystem.modules.additionalcharge.Infrastructure.entity;
using AirTicketSystem.modules.invoice.Infrastructure.entity;

namespace AirTicketSystem.shared.context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    // ── Geografía ──────────────────────────────────────────
    public DbSet<ContinentEntity> Continentes => Set<ContinentEntity>();
    public DbSet<CountryEntity> Paises => Set<CountryEntity>();
    public DbSet<RegionEntity> Regiones => Set<RegionEntity>();
    public DbSet<DepartmentEntity> Departamentos => Set<DepartmentEntity>();
    public DbSet<CityEntity> Ciudades => Set<CityEntity>();
    public DbSet<AirportEntity> Aeropuertos => Set<AirportEntity>();
    public DbSet<TerminalEntity> Terminales => Set<TerminalEntity>();
    public DbSet<GateEntity> PuertasEmbarque => Set<GateEntity>();

    // ── Catálogos ──────────────────────────────────────────
    public DbSet<GenderEntity> Generos => Set<GenderEntity>();
    public DbSet<DocumentTypeEntity> TiposDocumento => Set<DocumentTypeEntity>();
    public DbSet<PhoneTypeEntity> TiposTelefono => Set<PhoneTypeEntity>();
    public DbSet<EmailTypeEntity> TiposEmail => Set<EmailTypeEntity>();
    public DbSet<AddressTypeEntity> TiposDireccion => Set<AddressTypeEntity>();
    public DbSet<ContactRelationshipEntity> RelacionesContacto => Set<ContactRelationshipEntity>();

    // ── Personas y usuarios ────────────────────────────────
    public DbSet<PersonEntity> Personas => Set<PersonEntity>();
    public DbSet<PersonPhoneEntity> TelefonosPersona => Set<PersonPhoneEntity>();
    public DbSet<PersonEmailEntity> EmailsPersona => Set<PersonEmailEntity>();
    public DbSet<PersonAddressEntity> DireccionesPersona => Set<PersonAddressEntity>();
    public DbSet<RoleEntity> Roles => Set<RoleEntity>();
    public DbSet<UserEntity> Usuarios => Set<UserEntity>();
    public DbSet<AccessLogEntity> LogsAcceso => Set<AccessLogEntity>();
    public DbSet<ClientEntity> Clientes => Set<ClientEntity>();
    public DbSet<EmergencyContactEntity> ContactosEmergencia => Set<EmergencyContactEntity>();

    // ── Aerolíneas ─────────────────────────────────────────
    public DbSet<AirlineEntity> Aerolineas => Set<AirlineEntity>();
    public DbSet<AirlinePhoneEntity> TelefonosAerolinea => Set<AirlinePhoneEntity>();
    public DbSet<AirlineEmailEntity> EmailsAerolinea => Set<AirlineEmailEntity>();
    public DbSet<RouteEntity> Rutas => Set<RouteEntity>();

    // ── Flota ──────────────────────────────────────────────
    public DbSet<AircraftManufacturerEntity> FabricantesAvion => Set<AircraftManufacturerEntity>();
    public DbSet<AircraftModelEntity> ModelosAvion => Set<AircraftModelEntity>();
    public DbSet<AircraftEntity> Aviones => Set<AircraftEntity>();
    public DbSet<ServiceClassEntity> ClasesServicio => Set<ServiceClassEntity>();
    public DbSet<AircraftSeatEntity> AsientosAvion => Set<AircraftSeatEntity>();

    // ── Personal ───────────────────────────────────────────
    public DbSet<WorkerTypeEntity> TiposTrabajador => Set<WorkerTypeEntity>();
    public DbSet<SpecialtyEntity> Especialidades => Set<SpecialtyEntity>();
    public DbSet<WorkerEntity> Trabajadores => Set<WorkerEntity>();
    public DbSet<WorkerSpecialtyEntity> TrabajadorEspecialidades => Set<WorkerSpecialtyEntity>();
    public DbSet<PilotLicenseEntity> LicenciasPiloto => Set<PilotLicenseEntity>();
    public DbSet<PilotRatingEntity> HabilitacionesPiloto => Set<PilotRatingEntity>();

    // ── Vuelos ─────────────────────────────────────────────
    public DbSet<FlightEntity> Vuelos => Set<FlightEntity>();
    public DbSet<FlightCrewEntity> TripulacionVuelo => Set<FlightCrewEntity>();
    public DbSet<SeatAvailabilityEntity> DisponibilidadAsientos => Set<SeatAvailabilityEntity>();
    public DbSet<FlightHistoryEntity> HistorialVuelo => Set<FlightHistoryEntity>();

    // ── Tarifas y equipaje ─────────────────────────────────
    public DbSet<FareEntity> Tarifas => Set<FareEntity>();
    public DbSet<LuggageTypeEntity> TiposEquipaje => Set<LuggageTypeEntity>();
    public DbSet<LuggageRestrictionEntity> RestriccionesEquipaje => Set<LuggageRestrictionEntity>();

    // ── Reservas ───────────────────────────────────────────
    public DbSet<BookingEntity> Reservas => Set<BookingEntity>();
    public DbSet<BookingPassengerEntity> PasajerosReserva => Set<BookingPassengerEntity>();
    public DbSet<BookingHistoryEntity> HistorialReserva => Set<BookingHistoryEntity>();
    public DbSet<TicketEntity> Tiquetes => Set<TicketEntity>();
    public DbSet<LuggageEntity> Equipaje => Set<LuggageEntity>();

    // ── Check-in y pagos ───────────────────────────────────
    public DbSet<CheckInEntity> CheckIns => Set<CheckInEntity>();
    public DbSet<BoardingPassEntity> PasesAbordar => Set<BoardingPassEntity>();
    public DbSet<PaymentMethodEntity> MetodosPago => Set<PaymentMethodEntity>();
    public DbSet<PaymentEntity> Pagos => Set<PaymentEntity>();
    public DbSet<AdditionalChargeEntity> CargosAdicionales => Set<AdditionalChargeEntity>();
    public DbSet<InvoiceEntity> Facturas => Set<InvoiceEntity>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Aplica automáticamente TODAS las configuraciones IEntityTypeConfiguration
        // del ensamblado actual — no hay que registrar cada una a mano
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}