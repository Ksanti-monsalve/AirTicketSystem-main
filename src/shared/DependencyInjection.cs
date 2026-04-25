// src/shared/DependencyInjection.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AirTicketSystem.shared.context;
using AirTicketSystem.shared.helpers;
using AirTicketSystem.shared.UI;

// ── Domain repositories ──────────────────────────────────────────────────────
using AirTicketSystem.modules.additionalcharge.Domain.Repositories;
using AirTicketSystem.modules.addresstype.Domain.Repositories;
using AirTicketSystem.modules.aircraft.Domain.Repositories;
using AirTicketSystem.modules.aircraftmanufacturer.Domain.Repositories;
using AirTicketSystem.modules.aircraftmodel.Domain.Repositories;
using AirTicketSystem.modules.aircraftseat.Domain.Repositories;
using AirTicketSystem.modules.airline.Domain.Repositories;
using AirTicketSystem.modules.airport.Domain.Repositories;
using AirTicketSystem.modules.boardingpass.Domain.Repositories;
using AirTicketSystem.modules.booking.Domain.Repositories;
using AirTicketSystem.modules.bookinghistory.Domain.Repositories;
using AirTicketSystem.modules.bookingpassenger.Domain.Repositories;
using AirTicketSystem.modules.checkin.Domain.Repositories;
using AirTicketSystem.modules.city.Domain.Repositories;
using AirTicketSystem.modules.client.Domain.Repositories;
using AirTicketSystem.modules.contactrelationship.Domain.Repositories;
using AirTicketSystem.modules.continent.Domain.Repositories;
using AirTicketSystem.modules.country.Domain.Repositories;
using AirTicketSystem.modules.department.Domain.Repositories;
using AirTicketSystem.modules.documenttype.Domain.Repositories;
using AirTicketSystem.modules.emailtype.Domain.Repositories;
using AirTicketSystem.modules.fare.Domain.Repositories;
using AirTicketSystem.modules.flight.Domain.Repositories;
using AirTicketSystem.modules.flightcrew.Domain.Repositories;
using AirTicketSystem.modules.flighthistory.Domain.Repositories;
using AirTicketSystem.modules.gate.Domain.Repositories;
using AirTicketSystem.modules.gender.Domain.Repositories;
using AirTicketSystem.modules.invoice.Domain.Repositories;
using AirTicketSystem.modules.luggage.Domain.Repositories;
using AirTicketSystem.modules.luggagerestriction.Domain.Repositories;
using AirTicketSystem.modules.luggagetype.Domain.Repositories;
using AirTicketSystem.modules.payment.Domain.Repositories;
using AirTicketSystem.modules.paymentmethod.Domain.Repositories;
using AirTicketSystem.modules.person.Domain.Repositories;
using AirTicketSystem.modules.phonetype.Domain.Repositories;
using AirTicketSystem.modules.pilotlicense.Domain.Repositories;
using AirTicketSystem.modules.pilotrating.Domain.Repositories;
using AirTicketSystem.modules.region.Domain.Repositories;
using AirTicketSystem.modules.role.Domain.Repositories;
using AirTicketSystem.modules.route.Domain.Repositories;
using AirTicketSystem.modules.seatavailability.Domain.Repositories;
using AirTicketSystem.modules.serviceclass.Domain.Repositories;
using AirTicketSystem.modules.specialty.Domain.Repositories;
using AirTicketSystem.modules.terminal.Domain.Repositories;
using AirTicketSystem.modules.ticket.Domain.Repositories;
using AirTicketSystem.modules.user.Domain.Repositories;
using AirTicketSystem.modules.worker.Domain.Repositories;
using AirTicketSystem.modules.workertype.Domain.Repositories;

// ── Infrastructure repositories ──────────────────────────────────────────────
using AirTicketSystem.modules.additionalcharge.Infrastructure.repository;
using AirTicketSystem.modules.addresstype.Infrastructure.repository;
using AirTicketSystem.modules.aircraft.Infrastructure.repository;
using AirTicketSystem.modules.aircraftmanufacturer.Infrastructure.repository;
using AirTicketSystem.modules.aircraftmodel.Infrastructure.repository;
using AirTicketSystem.modules.aircraftseat.Infrastructure.repository;
using AirTicketSystem.modules.airline.Infrastructure.repository;
using AirTicketSystem.modules.airport.Infrastructure.repository;
using AirTicketSystem.modules.boardingpass.Infrastructure.repository;
using AirTicketSystem.modules.booking.Infrastructure.repository;
using AirTicketSystem.modules.bookinghistory.Infrastructure.repository;
using AirTicketSystem.modules.bookingpassenger.Infrastructure.repository;
using AirTicketSystem.modules.checkin.Infrastructure.repository;
using AirTicketSystem.modules.city.Infrastructure.repository;
using AirTicketSystem.modules.client.Infrastructure.repository;
using AirTicketSystem.modules.contactrelationship.Infrastructure.repository;
using AirTicketSystem.modules.continent.Infrastructure.repository;
using AirTicketSystem.modules.country.Infrastructure.repository;
using AirTicketSystem.modules.department.Infrastructure.repository;
using AirTicketSystem.modules.documenttype.Infrastructure.repository;
using AirTicketSystem.modules.emailtype.Infrastructure.repository;
using AirTicketSystem.modules.fare.Infrastructure.repository;
using AirTicketSystem.modules.flight.Infrastructure.repository;
using AirTicketSystem.modules.flightcrew.Infrastructure.repository;
using AirTicketSystem.modules.flighthistory.Infrastructure.repository;
using AirTicketSystem.modules.gate.Infrastructure.repository;
using AirTicketSystem.modules.gender.Infrastructure.repository;
using AirTicketSystem.modules.invoice.Infrastructure.repository;
using AirTicketSystem.modules.luggage.Infrastructure.repository;
using AirTicketSystem.modules.luggagerestriction.Infrastructure.repository;
using AirTicketSystem.modules.luggagetype.Infrastructure.repository;
using AirTicketSystem.modules.payment.Infrastructure.repository;
using AirTicketSystem.modules.paymentmethod.Infrastructure.repository;
using AirTicketSystem.modules.person.Infrastructure.repository;
using AirTicketSystem.modules.phonetype.Infrastructure.repository;
using AirTicketSystem.modules.pilotlicense.Infrastructure.repository;
using AirTicketSystem.modules.pilotrating.Infrastructure.repository;
using AirTicketSystem.modules.region.Infrastructure.repository;
using AirTicketSystem.modules.role.Infrastructure.repository;
using AirTicketSystem.modules.route.Infrastructure.repository;
using AirTicketSystem.modules.seatavailability.Infrastructure.repository;
using AirTicketSystem.modules.serviceclass.Infrastructure.repository;
using AirTicketSystem.modules.specialty.Infrastructure.repository;
using AirTicketSystem.modules.terminal.Infrastructure.repository;
using AirTicketSystem.modules.ticket.Infrastructure.repository;
using AirTicketSystem.modules.user.Infrastructure.repository;
using AirTicketSystem.modules.worker.Infrastructure.repository;
using AirTicketSystem.modules.workertype.Infrastructure.repository;

// ── Use Cases ─────────────────────────────────────────────────────────────────
using AirTicketSystem.modules.user.Application.UseCases;
using AirTicketSystem.modules.continent.Application.UseCases;
using AirTicketSystem.modules.country.Application.UseCases;
using AirTicketSystem.modules.region.Application.UseCases;
using AirTicketSystem.modules.department.Application.UseCases;
using AirTicketSystem.modules.city.Application.UseCases;
using AirTicketSystem.modules.gender.Application.UseCases;
using AirTicketSystem.modules.documenttype.Application.UseCases;
using AirTicketSystem.modules.addresstype.Application.UseCases;
using AirTicketSystem.modules.phonetype.Application.UseCases;
using AirTicketSystem.modules.emailtype.Application.UseCases;
using AirTicketSystem.modules.contactrelationship.Application.UseCases;
using AirTicketSystem.modules.serviceclass.Application.UseCases;
using AirTicketSystem.modules.workertype.Application.UseCases;
using AirTicketSystem.modules.specialty.Application.UseCases;
using AirTicketSystem.modules.luggagetype.Application.UseCases;
using AirTicketSystem.modules.aircraftmanufacturer.Application.UseCases;
using AirTicketSystem.modules.aircraftmodel.Application.UseCases;
using AirTicketSystem.modules.aircraft.Application.UseCases;
using AirTicketSystem.modules.aircraftseat.Application.UseCases;
using AirTicketSystem.modules.airport.Application.UseCases;
using AirTicketSystem.modules.terminal.Application.UseCases;
using AirTicketSystem.modules.gate.Application.UseCases;
using AirTicketSystem.modules.airline.Application.UseCases;
using AirTicketSystem.modules.route.Application.UseCases;
using AirTicketSystem.modules.fare.Application.UseCases;
using AirTicketSystem.modules.luggagerestriction.Application.UseCases;
using AirTicketSystem.modules.flight.Application.UseCases;
using AirTicketSystem.modules.booking.Application.UseCases;
using AirTicketSystem.modules.bookingpassenger.Application.UseCases;
using AirTicketSystem.modules.checkin.Application.UseCases;
using AirTicketSystem.modules.payment.Application.UseCases;
using AirTicketSystem.modules.invoice.Application.UseCases;
using AirTicketSystem.modules.role.Application.UseCases;
using AirTicketSystem.modules.client.Application.UseCases;
using AirTicketSystem.modules.ticket.Application.UseCases;
using AirTicketSystem.modules.boardingpass.Application.UseCases;
using AirTicketSystem.modules.seatavailability.Application.UseCases;
using AirTicketSystem.modules.worker.Application.UseCases;
using AirTicketSystem.modules.flightcrew.Application.UseCases;
using AirTicketSystem.modules.pilotlicense.Application.UseCases;
using AirTicketSystem.modules.pilotrating.Application.UseCases;
using AirTicketSystem.modules.person.Application.UseCases;
using AirTicketSystem.modules.luggage.Application.UseCases;
using AirTicketSystem.modules.additionalcharge.Application.UseCases;
using AirTicketSystem.modules.flighthistory.Application.UseCases;
using AirTicketSystem.modules.paymentmethod.Application.UseCases;
using AirTicketSystem.modules.luggagerestriction.Application.UseCases;

namespace AirTicketSystem.shared;

public static class DependencyInjection
{
    public static IServiceCollection AddAirTicketSystem(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // DbContext
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException(
                "No se encontró 'DefaultConnection' en appsettings.json");

        var serverVersion = MySqlVersionResolver.Resolve(connectionString);

        services.AddDbContext<AppDbContext>(options =>
            options.UseMySql(connectionString, serverVersion, mySql =>
                mySql.EnableRetryOnFailure(
                    maxRetryCount: 3,
                    maxRetryDelay: TimeSpan.FromSeconds(5),
                    errorNumbersToAdd: null)),
            ServiceLifetime.Scoped);

        // Session (singleton — dura toda la aplicación)
        services.AddSingleton<SessionContext>();

        // ── Repositories ────────────────────────────────────────────────────
        services.AddScoped<IAdditionalChargeRepository,   AdditionalChargeRepository>();
        services.AddScoped<IAddressTypeRepository,        AddressTypeRepository>();
        services.AddScoped<IAircraftRepository,           AircraftRepository>();
        services.AddScoped<IAircraftManufacturerRepository, AircraftManufacturerRepository>();
        services.AddScoped<IAircraftModelRepository,      AircraftModelRepository>();
        services.AddScoped<IAircraftSeatRepository,       AircraftSeatRepository>();
        services.AddScoped<IAirlineRepository,            AirlineRepository>();
        services.AddScoped<IAirlineEmailRepository,       AirlineEmailRepository>();
        services.AddScoped<IAirlinePhoneRepository,       AirlinePhoneRepository>();
        services.AddScoped<IAirportRepository,            AirportRepository>();
        services.AddScoped<IBoardingPassRepository,       BoardingPassRepository>();
        services.AddScoped<IBookingRepository,            BookingRepository>();
        services.AddScoped<IBookingHistoryRepository,     BookingHistoryRepository>();
        services.AddScoped<IBookingPassengerRepository,   BookingPassengerRepository>();
        services.AddScoped<ICheckInRepository,            CheckInRepository>();
        services.AddScoped<ICityRepository,               CityRepository>();
        services.AddScoped<IClientRepository,             ClientRepository>();
        services.AddScoped<IEmergencyContactRepository,   EmergencyContactRepository>();
        services.AddScoped<IContactRelationshipRepository, ContactRelationshipRepository>();
        services.AddScoped<IContinentRepository,          ContinentRepository>();
        services.AddScoped<ICountryRepository,            CountryRepository>();
        services.AddScoped<IDepartmentRepository,         DepartmentRepository>();
        services.AddScoped<IDocumentTypeRepository,       DocumentTypeRepository>();
        services.AddScoped<IEmailTypeRepository,          EmailTypeRepository>();
        services.AddScoped<IFareRepository,               FareRepository>();
        services.AddScoped<IFlightRepository,             FlightRepository>();
        services.AddScoped<IFlightCrewRepository,         FlightCrewRepository>();
        services.AddScoped<IFlightHistoryRepository,      FlightHistoryRepository>();
        services.AddScoped<IGateRepository,               GateRepository>();
        services.AddScoped<IGenderRepository,             GenderRepository>();
        services.AddScoped<IInvoiceRepository,            InvoiceRepository>();
        services.AddScoped<ILuggageRepository,            LuggageRepository>();
        services.AddScoped<ILuggageRestrictionRepository, LuggageRestrictionRepository>();
        services.AddScoped<ILuggageTypeRepository,        LuggageTypeRepository>();
        services.AddScoped<IPaymentRepository,            PaymentRepository>();
        services.AddScoped<IPaymentMethodRepository,      PaymentMethodRepository>();
        services.AddScoped<IPersonRepository,             PersonRepository>();
        services.AddScoped<IPersonAddressRepository,      PersonAddressRepository>();
        services.AddScoped<IPersonEmailRepository,        PersonEmailRepository>();
        services.AddScoped<IPersonPhoneRepository,        PersonPhoneRepository>();
        services.AddScoped<IPhoneTypeRepository,          PhoneTypeRepository>();
        services.AddScoped<IPilotLicenseRepository,       PilotLicenseRepository>();
        services.AddScoped<IPilotRatingRepository,        PilotRatingRepository>();
        services.AddScoped<IRegionRepository,             RegionRepository>();
        services.AddScoped<IRoleRepository,               RoleRepository>();
        services.AddScoped<IRouteRepository,              RouteRepository>();
        services.AddScoped<ISeatAvailabilityRepository,   SeatAvailabilityRepository>();
        services.AddScoped<IServiceClassRepository,       ServiceClassRepository>();
        services.AddScoped<ISpecialtyRepository,          SpecialtyRepository>();
        services.AddScoped<ITerminalRepository,           TerminalRepository>();
        services.AddScoped<ITicketRepository,             TicketRepository>();
        services.AddScoped<IAccessLogRepository,          AccessLogRepository>();
        services.AddScoped<IUserRepository,               UserRepository>();
        services.AddScoped<IWorkerRepository,             WorkerRepository>();
        services.AddScoped<IWorkerSpecialtyRepository,    WorkerSpecialtyRepository>();
        services.AddScoped<IWorkerTypeRepository,         WorkerTypeRepository>();

        // ── Use Cases ───────────────────────────────────────────────────────

        // Auth / User
        services.AddScoped<LoginUseCase>();
        services.AddScoped<RegisterClientUseCase>();
        services.AddScoped<SeedAdminUseCase>();

        // Continent
        services.AddScoped<GetAllContinentsUseCase>();
        services.AddScoped<GetContinentByIdUseCase>();
        services.AddScoped<CreateContinentUseCase>();
        services.AddScoped<UpdateContinentUseCase>();
        services.AddScoped<DeleteContinentUseCase>();

        // Country
        services.AddScoped<GetAllCountriesUseCase>();
        services.AddScoped<GetCountryByIdUseCase>();
        services.AddScoped<GetCountriesByContinentUseCase>();
        services.AddScoped<CreateCountryUseCase>();
        services.AddScoped<UpdateCountryUseCase>();
        services.AddScoped<DeleteCountryUseCase>();

        // Region
        services.AddScoped<GetAllRegionsUseCase>();
        services.AddScoped<GetRegionByIdUseCase>();
        services.AddScoped<GetRegionsByCountryUseCase>();
        services.AddScoped<CreateRegionUseCase>();
        services.AddScoped<UpdateRegionUseCase>();
        services.AddScoped<DeleteRegionUseCase>();

        // Department
        services.AddScoped<GetAllDepartmentsUseCase>();
        services.AddScoped<GetDepartmentByIdUseCase>();
        services.AddScoped<GetDepartmentsByRegionUseCase>();
        services.AddScoped<CreateDepartmentUseCase>();
        services.AddScoped<UpdateDepartmentUseCase>();
        services.AddScoped<DeleteDepartmentUseCase>();

        // City
        services.AddScoped<GetAllCitiesUseCase>();
        services.AddScoped<GetCityByIdUseCase>();
        services.AddScoped<GetCitiesByDepartmentUseCase>();
        services.AddScoped<CreateCityUseCase>();
        services.AddScoped<UpdateCityUseCase>();
        services.AddScoped<DeleteCityUseCase>();

        // Gender
        services.AddScoped<GetAllGendersUseCase>();
        services.AddScoped<GetGenderByIdUseCase>();
        services.AddScoped<CreateGenderUseCase>();
        services.AddScoped<UpdateGenderUseCase>();
        services.AddScoped<DeleteGenderUseCase>();

        // DocumentType
        services.AddScoped<GetAllDocumentTypesUseCase>();
        services.AddScoped<GetDocumentTypeByIdUseCase>();
        services.AddScoped<CreateDocumentTypeUseCase>();
        services.AddScoped<UpdateDocumentTypeUseCase>();
        services.AddScoped<DeleteDocumentTypeUseCase>();

        // AddressType
        services.AddScoped<GetAllAddressTypesUseCase>();
        services.AddScoped<GetAddressTypeByIdUseCase>();
        services.AddScoped<CreateAddressTypeUseCase>();
        services.AddScoped<UpdateAddressTypeUseCase>();
        services.AddScoped<DeleteAddressTypeUseCase>();

        // PhoneType
        services.AddScoped<GetAllPhoneTypesUseCase>();
        services.AddScoped<GetPhoneTypeByIdUseCase>();
        services.AddScoped<CreatePhoneTypeUseCase>();
        services.AddScoped<UpdatePhoneTypeUseCase>();
        services.AddScoped<DeletePhoneTypeUseCase>();

        // EmailType
        services.AddScoped<GetAllEmailTypesUseCase>();
        services.AddScoped<GetEmailTypeByIdUseCase>();
        services.AddScoped<CreateEmailTypeUseCase>();
        services.AddScoped<UpdateEmailTypeUseCase>();
        services.AddScoped<DeleteEmailTypeUseCase>();

        // ContactRelationship
        services.AddScoped<GetAllContactRelationshipsUseCase>();
        services.AddScoped<GetContactRelationshipByIdUseCase>();
        services.AddScoped<CreateContactRelationshipUseCase>();
        services.AddScoped<UpdateContactRelationshipUseCase>();
        services.AddScoped<DeleteContactRelationshipUseCase>();

        // ServiceClass
        services.AddScoped<GetAllServiceClassesUseCase>();
        services.AddScoped<GetServiceClassByIdUseCase>();
        services.AddScoped<CreateServiceClassUseCase>();
        services.AddScoped<UpdateServiceClassUseCase>();
        services.AddScoped<DeleteServiceClassUseCase>();

        // WorkerType
        services.AddScoped<GetAllWorkerTypesUseCase>();
        services.AddScoped<GetWorkerTypeByIdUseCase>();
        services.AddScoped<CreateWorkerTypeUseCase>();
        services.AddScoped<UpdateWorkerTypeUseCase>();
        services.AddScoped<DeleteWorkerTypeUseCase>();

        // Specialty
        services.AddScoped<GetAllSpecialtiesUseCase>();
        services.AddScoped<GetSpecialtyByIdUseCase>();
        services.AddScoped<GetGeneralSpecialtiesUseCase>();
        services.AddScoped<GetSpecialtiesByWorkerTypeUseCase>();
        services.AddScoped<CreateSpecialtyUseCase>();
        services.AddScoped<UpdateSpecialtyUseCase>();
        services.AddScoped<DeleteSpecialtyUseCase>();

        // LuggageType
        services.AddScoped<GetAllLuggageTypesUseCase>();
        services.AddScoped<GetLuggageTypeByIdUseCase>();
        services.AddScoped<CreateLuggageTypeUseCase>();
        services.AddScoped<UpdateLuggageTypeUseCase>();
        services.AddScoped<DeleteLuggageTypeUseCase>();

        // AircraftManufacturer
        services.AddScoped<GetAllAircraftManufacturersUseCase>();
        services.AddScoped<GetAircraftManufacturerByIdUseCase>();
        services.AddScoped<GetAircraftManufacturersByCountryUseCase>();
        services.AddScoped<CreateAircraftManufacturerUseCase>();
        services.AddScoped<UpdateAircraftManufacturerUseCase>();
        services.AddScoped<DeleteAircraftManufacturerUseCase>();

        // AircraftModel
        services.AddScoped<GetAllAircraftModelsUseCase>();
        services.AddScoped<GetAircraftModelByIdUseCase>();
        services.AddScoped<GetAircraftModelsByManufacturerUseCase>();
        services.AddScoped<CreateAircraftModelUseCase>();
        services.AddScoped<UpdateAircraftModelUseCase>();
        services.AddScoped<DeleteAircraftModelUseCase>();

        // Aircraft
        services.AddScoped<GetAllAircraftUseCase>();
        services.AddScoped<GetAircraftByIdUseCase>();
        services.AddScoped<GetAircraftByMatriculaUseCase>();
        services.AddScoped<GetAircraftByAirlineUseCase>();
        services.AddScoped<GetAvailableAircraftUseCase>();
        services.AddScoped<GetAircraftWithUrgentMaintenanceUseCase>();
        services.AddScoped<CreateAircraftUseCase>();
        services.AddScoped<UpdateAircraftUseCase>();
        services.AddScoped<SendToMaintenanceUseCase>();
        services.AddScoped<RegisterMaintenanceUseCase>();
        services.AddScoped<RegisterLandingUseCase>();
        services.AddScoped<DecommissionAircraftUseCase>();
        services.AddScoped<ReactivateAircraftUseCase>();
        services.AddScoped<DeleteAircraftUseCase>();

        // AircraftSeat
        services.AddScoped<GetSeatsByAircraftUseCase>();
        services.AddScoped<GetSeatsByAircraftAndClassUseCase>();
        services.AddScoped<GetAircraftSeatByIdUseCase>();
        services.AddScoped<CreateAircraftSeatUseCase>();
        services.AddScoped<UpdateAircraftSeatUseCase>();
        services.AddScoped<ActivateAircraftSeatUseCase>();
        services.AddScoped<DeactivateAircraftSeatUseCase>();
        services.AddScoped<DeleteAircraftSeatUseCase>();

        // Airport
        services.AddScoped<GetAllAirportsUseCase>();
        services.AddScoped<GetAirportByIdUseCase>();
        services.AddScoped<GetAirportByIataUseCase>();
        services.AddScoped<GetActiveAirportsUseCase>();
        services.AddScoped<GetAirportsByCityUseCase>();
        services.AddScoped<CreateAirportUseCase>();
        services.AddScoped<UpdateAirportUseCase>();
        services.AddScoped<ActivateAirportUseCase>();
        services.AddScoped<DeactivateAirportUseCase>();
        services.AddScoped<DeleteAirportUseCase>();

        // Terminal
        services.AddScoped<GetTerminalByIdUseCase>();
        services.AddScoped<GetTerminalsByAirportUseCase>();
        services.AddScoped<CreateTerminalUseCase>();
        services.AddScoped<UpdateTerminalUseCase>();
        services.AddScoped<DeleteTerminalUseCase>();

        // Gate
        services.AddScoped<GetGateByIdUseCase>();
        services.AddScoped<GetGatesByTerminalUseCase>();
        services.AddScoped<GetActiveGatesByTerminalUseCase>();
        services.AddScoped<CreateGateUseCase>();
        services.AddScoped<UpdateGateUseCase>();
        services.AddScoped<ActivateGateUseCase>();
        services.AddScoped<DeactivateGateUseCase>();
        services.AddScoped<DeleteGateUseCase>();

        // Airline
        services.AddScoped<GetAllAirlinesUseCase>();
        services.AddScoped<GetAirlineByIdUseCase>();
        services.AddScoped<GetAirlineByIataUseCase>();
        services.AddScoped<GetActiveAirlinesUseCase>();
        services.AddScoped<CreateAirlineUseCase>();
        services.AddScoped<UpdateAirlineUseCase>();
        services.AddScoped<ActivateAirlineUseCase>();
        services.AddScoped<DeactivateAirlineUseCase>();
        services.AddScoped<DeleteAirlineUseCase>();
        services.AddScoped<AddAirlineEmailUseCase>();
        services.AddScoped<RemoveAirlineEmailUseCase>();
        services.AddScoped<AddAirlinePhoneUseCase>();
        services.AddScoped<RemoveAirlinePhoneUseCase>();

        // Route
        services.AddScoped<GetAllRoutesUseCase>();
        services.AddScoped<GetRouteByIdUseCase>();
        services.AddScoped<GetActiveRoutesUseCase>();
        services.AddScoped<GetRoutesByAirlineUseCase>();
        services.AddScoped<GetRoutesByOriginUseCase>();
        services.AddScoped<GetRoutesByDestinationUseCase>();
        services.AddScoped<SearchRoutesUseCase>();
        services.AddScoped<CreateRouteUseCase>();
        services.AddScoped<UpdateRouteUseCase>();
        services.AddScoped<ActivateRouteUseCase>();
        services.AddScoped<DeactivateRouteUseCase>();
        services.AddScoped<DeleteRouteUseCase>();

        // Fare
        services.AddScoped<GetAllFaresUseCase>();
        services.AddScoped<GetFareByIdUseCase>();
        services.AddScoped<GetActiveFaresUseCase>();
        services.AddScoped<GetFaresByRouteUseCase>();
        services.AddScoped<GetActiveFaresByRouteUseCase>();
        services.AddScoped<CreateFareUseCase>();
        services.AddScoped<UpdateFareUseCase>();
        services.AddScoped<ActivateFareUseCase>();
        services.AddScoped<DeactivateFareUseCase>();
        services.AddScoped<DeleteFareUseCase>();

        // LuggageRestriction
        services.AddScoped<GetAllRestrictionsUseCase>();
        services.AddScoped<GetLuggageRestrictionByIdUseCase>();
        services.AddScoped<GetRestrictionsByFareUseCase>();
        services.AddScoped<CreateLuggageRestrictionUseCase>();
        services.AddScoped<UpdateLuggageRestrictionUseCase>();
        services.AddScoped<DeleteLuggageRestrictionUseCase>();

        // Flight
        services.AddScoped<GetAllFlightsUseCase>();
        services.AddScoped<GetFlightByIdUseCase>();
        services.AddScoped<GetFlightByNumberAndDateUseCase>();
        services.AddScoped<GetFlightsByDateUseCase>();
        services.AddScoped<GetFlightsByRouteUseCase>();
        services.AddScoped<GetScheduledFlightsUseCase>();
        services.AddScoped<SearchFlightsUseCase>();
        services.AddScoped<GetFlightsWithOpenCheckinUseCase>();
        services.AddScoped<CreateFlightUseCase>();
        services.AddScoped<UpdateFlightUseCase>();
        services.AddScoped<AssignGateToFlightUseCase>();
        services.AddScoped<OpenCheckinUseCase>();
        services.AddScoped<StartBoardingUseCase>();
        services.AddScoped<StartFlightUseCase>();
        services.AddScoped<RegisterLandingFlightUseCase>();
        services.AddScoped<DelayFlightUseCase>();
        services.AddScoped<CancelFlightUseCase>();
        services.AddScoped<DivertFlightUseCase>();
        services.AddScoped<DeleteFlightUseCase>();

        // Booking
        services.AddScoped<CreateBookingUseCase>();
        services.AddScoped<GetBookingByIdUseCase>();
        services.AddScoped<GetBookingByCodigoUseCase>();
        services.AddScoped<GetBookingsByClienteUseCase>();
        services.AddScoped<ConfirmBookingUseCase>();
        services.AddScoped<CancelBookingUseCase>();
        services.AddScoped<ExpireBookingUseCase>();
        services.AddScoped<ExtendBookingUseCase>();
        services.AddScoped<UpdateBookingObservationsUseCase>();
        services.AddScoped<DeleteBookingUseCase>();

        // BookingPassenger
        services.AddScoped<AddPassengerUseCase>();
        services.AddScoped<GetPassengersByBookingUseCase>();
        services.AddScoped<AssignSeatUseCase>();
        services.AddScoped<ChangeSeatUseCase>();
        services.AddScoped<AirTicketSystem.modules.bookingpassenger.Application.UseCases.ReleaseSeatUseCase>();

        // CheckIn
        services.AddScoped<CreateVirtualCheckInUseCase>();
        services.AddScoped<CreatePresentialCheckInUseCase>();
        services.AddScoped<CompleteCheckInUseCase>();
        services.AddScoped<CancelCheckInUseCase>();
        services.AddScoped<GetCheckInByIdUseCase>();
        services.AddScoped<GetCheckInByPassengerUseCase>();

        // Payment
        services.AddScoped<CreatePaymentUseCase>();
        services.AddScoped<ApprovePaymentUseCase>();
        services.AddScoped<RejectPaymentUseCase>();
        services.AddScoped<RefundPaymentUseCase>();
        services.AddScoped<RetryPaymentUseCase>();
        services.AddScoped<GetPaymentsByBookingUseCase>();

        // Invoice
        services.AddScoped<GenerateInvoiceUseCase>();
        services.AddScoped<GetInvoiceByBookingUseCase>();
        services.AddScoped<GetInvoiceByNumeroUseCase>();
        services.AddScoped<UpdateInvoiceAddressUseCase>();

        // Role
        services.AddScoped<GetAllRolesUseCase>();
        services.AddScoped<GetRoleByIdUseCase>();
        services.AddScoped<CreateRoleUseCase>();
        services.AddScoped<UpdateRoleUseCase>();
        services.AddScoped<DeleteRoleUseCase>();

        // User (admin ops — Login y Register ya registrados arriba)
        services.AddScoped<CreateUserUseCase>();
        services.AddScoped<GetUserByIdUseCase>();
        services.AddScoped<GetAllUsersUseCase>();
        services.AddScoped<GetUserByUsernameUseCase>();
        services.AddScoped<ChangePasswordUseCase>();
        services.AddScoped<ChangeRoleUseCase>();
        services.AddScoped<ActivateUserUseCase>();
        services.AddScoped<DeactivateUserUseCase>();
        services.AddScoped<DeleteUserUseCase>();

        // Client
        services.AddScoped<CreateClientUseCase>();
        services.AddScoped<GetClientByIdUseCase>();
        services.AddScoped<GetAllClientsUseCase>();
        services.AddScoped<GetActiveClientsUseCase>();
        services.AddScoped<ActivateClientUseCase>();
        services.AddScoped<DeactivateClientUseCase>();
        services.AddScoped<DeleteClientUseCase>();
        services.AddScoped<AddEmergencyContactUseCase>();
        services.AddScoped<SetPrincipalEmergencyContactUseCase>();
        services.AddScoped<UpdateEmergencyContactUseCase>();
        services.AddScoped<DeleteEmergencyContactUseCase>();

        // Ticket
        services.AddScoped<EmitTicketUseCase>();
        services.AddScoped<GetTicketByCodeUseCase>();
        services.AddScoped<GetTicketByPassengerUseCase>();
        services.AddScoped<CheckInTicketUseCase>();
        services.AddScoped<BoardTicketUseCase>();
        services.AddScoped<VoidTicketUseCase>();

        // BoardingPass
        services.AddScoped<GenerateBoardingPassUseCase>();
        services.AddScoped<GetBoardingPassByCodeUseCase>();
        services.AddScoped<GetBoardingPassByCheckInUseCase>();
        services.AddScoped<AssignGateUseCase>();
        services.AddScoped<AssignBoardingTimeUseCase>();

        // SeatAvailability
        services.AddScoped<GetAvailableSeatsByFlightUseCase>();
        services.AddScoped<ReserveSeatUseCase>();
        services.AddScoped<AirTicketSystem.modules.seatavailability.Application.UseCases.ReleaseSeatUseCase>();
        services.AddScoped<BlockSeatUseCase>();

        // Worker
        services.AddScoped<CreateWorkerUseCase>();
        services.AddScoped<GetAllWorkersUseCase>();
        services.AddScoped<GetWorkerByIdUseCase>();
        services.AddScoped<GetWorkerByPersonUseCase>();
        services.AddScoped<GetWorkersByAirlineUseCase>();
        services.AddScoped<GetWorkersByAirportUseCase>();
        services.AddScoped<GetWorkersByWorkerTypeUseCase>();
        services.AddScoped<GetActiveWorkersUseCase>();
        services.AddScoped<GetQualifiedPilotsUseCase>();
        services.AddScoped<DeactivateWorkerUseCase>();
        services.AddScoped<ActivateWorkerUseCase>();
        services.AddScoped<DeleteWorkerUseCase>();
        services.AddScoped<UpdateWorkerSalaryUseCase>();
        services.AddScoped<UpdateWorkerAirportUseCase>();
        services.AddScoped<AssignUserToWorkerUseCase>();
        services.AddScoped<AssignWorkerSpecialtyUseCase>();
        services.AddScoped<RemoveWorkerSpecialtyUseCase>();

        // FlightCrew
        services.AddScoped<AssignCrewMemberUseCase>();
        services.AddScoped<RemoveCrewMemberUseCase>();
        services.AddScoped<GetCrewByFlightUseCase>();
        services.AddScoped<ValidateFlightCrewUseCase>();
        services.AddScoped<DeleteFlightCrewUseCase>();

        // PilotLicense
        services.AddScoped<CreatePilotLicenseUseCase>();
        services.AddScoped<DeletePilotLicenseUseCase>();
        services.AddScoped<GetLicensesByWorkerUseCase>();
        services.AddScoped<GetLicensesExpiringSoonUseCase>();
        services.AddScoped<GetPilotLicenseByIdUseCase>();
        services.AddScoped<GetVigenteLicensesUseCase>();
        services.AddScoped<ReactivatePilotLicenseUseCase>();
        services.AddScoped<RenewPilotLicenseUseCase>();
        services.AddScoped<SuspendPilotLicenseUseCase>();

        // PilotRating (Habilitaciones)
        services.AddScoped<CreatePilotRatingUseCase>();
        services.AddScoped<DeletePilotRatingUseCase>();
        services.AddScoped<GetPilotRatingByIdUseCase>();
        services.AddScoped<GetRatingsByAircraftModelUseCase>();
        services.AddScoped<GetRatingsByLicenseUseCase>();
        services.AddScoped<GetVigenteRatingsUseCase>();
        services.AddScoped<RenewPilotRatingUseCase>();

        // Person
        services.AddScoped<CreatePersonUseCase>();
        services.AddScoped<GetAllPersonsUseCase>();
        services.AddScoped<GetPersonByIdUseCase>();
        services.AddScoped<GetPersonByDocumentUseCase>();
        services.AddScoped<UpdatePersonUseCase>();
        services.AddScoped<DeletePersonUseCase>();
        services.AddScoped<AddPersonPhoneUseCase>();
        services.AddScoped<DeletePersonPhoneUseCase>();
        services.AddScoped<SetPrincipalPersonPhoneUseCase>();
        services.AddScoped<AddPersonEmailUseCase>();
        services.AddScoped<DeletePersonEmailUseCase>();
        services.AddScoped<SetPrincipalPersonEmailUseCase>();
        services.AddScoped<AddPersonAddressUseCase>();
        services.AddScoped<DeletePersonAddressUseCase>();
        services.AddScoped<SetPrincipalPersonAddressUseCase>();

        // Luggage
        services.AddScoped<RegisterLuggageUseCase>();
        services.AddScoped<GetLuggageByPassengerUseCase>();
        services.AddScoped<ProcessCheckInLuggageUseCase>();
        services.AddScoped<ReportDamagedLuggageUseCase>();
        services.AddScoped<ReportLostLuggageUseCase>();
        services.AddScoped<SendToBaggageUseCase>();

        // AdditionalCharge
        services.AddScoped<CreateAdditionalChargeUseCase>();
        services.AddScoped<DeleteAdditionalChargeUseCase>();
        services.AddScoped<GetAdditionalChargeByIdUseCase>();
        services.AddScoped<GetAllAdditionalChargesUseCase>();
        services.AddScoped<GetChargesByBookingUseCase>();
        services.AddScoped<GetTotalChargesByBookingUseCase>();

        // FlightHistory
        services.AddScoped<GetFlightHistoryUseCase>();
        services.AddScoped<GetLastFlightChangeUseCase>();

        // PaymentMethod
        services.AddScoped<GetAllPaymentMethodsUseCase>();
        services.AddScoped<CreatePaymentMethodUseCase>();
        services.AddScoped<GetPaymentMethodByIdUseCase>();
        services.AddScoped<UpdatePaymentMethodUseCase>();

        // Seed
        services.AddSingleton<SeedFullDataUseCase>(sp => new SeedFullDataUseCase(sp));

        return services;
    }
}
