// src/shared/SeedFullDataUseCase.cs
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using AirTicketSystem.shared.context;
using AirTicketSystem.shared.helpers;

// Domains
using AirTicketSystem.modules.gender.Domain.aggregate;
using AirTicketSystem.modules.gender.Domain.Repositories;
using AirTicketSystem.modules.documenttype.Domain.aggregate;
using AirTicketSystem.modules.documenttype.Domain.Repositories;
using AirTicketSystem.modules.phonetype.Domain.aggregate;
using AirTicketSystem.modules.phonetype.Domain.Repositories;
using AirTicketSystem.modules.emailtype.Domain.aggregate;
using AirTicketSystem.modules.emailtype.Domain.Repositories;
using AirTicketSystem.modules.addresstype.Domain.aggregate;
using AirTicketSystem.modules.addresstype.Domain.Repositories;
using AirTicketSystem.modules.contactrelationship.Domain.aggregate;
using AirTicketSystem.modules.contactrelationship.Domain.Repositories;
using AirTicketSystem.modules.luggagetype.Domain.aggregate;
using AirTicketSystem.modules.luggagetype.Domain.Repositories;
using AirTicketSystem.modules.serviceclass.Domain.aggregate;
using AirTicketSystem.modules.serviceclass.Domain.Repositories;
using AirTicketSystem.modules.workertype.Domain.aggregate;
using AirTicketSystem.modules.workertype.Domain.Repositories;
using AirTicketSystem.modules.specialty.Domain.aggregate;
using AirTicketSystem.modules.specialty.Domain.Repositories;
using AirTicketSystem.modules.paymentmethod.Domain.aggregate;
using AirTicketSystem.modules.paymentmethod.Domain.Repositories;
using AirTicketSystem.modules.continent.Domain.aggregate;
using AirTicketSystem.modules.continent.Domain.Repositories;
using AirTicketSystem.modules.country.Domain.aggregate;
using AirTicketSystem.modules.country.Domain.Repositories;
using AirTicketSystem.modules.region.Domain.aggregate;
using AirTicketSystem.modules.region.Domain.Repositories;
using AirTicketSystem.modules.department.Domain.aggregate;
using AirTicketSystem.modules.department.Domain.Repositories;
using AirTicketSystem.modules.city.Domain.aggregate;
using AirTicketSystem.modules.city.Domain.Repositories;
using AirTicketSystem.modules.aircraftmanufacturer.Domain.aggregate;
using AirTicketSystem.modules.aircraftmanufacturer.Domain.Repositories;
using AirTicketSystem.modules.aircraftmodel.Domain.aggregate;
using AirTicketSystem.modules.aircraftmodel.Domain.Repositories;
using AirTicketSystem.modules.airline.Domain.aggregate;
using AirTicketSystem.modules.airline.Domain.Repositories;
using AirTicketSystem.modules.airport.Domain.aggregate;
using AirTicketSystem.modules.airport.Domain.Repositories;
using AirTicketSystem.modules.terminal.Domain.aggregate;
using AirTicketSystem.modules.terminal.Domain.Repositories;
using AirTicketSystem.modules.gate.Domain.aggregate;
using AirTicketSystem.modules.gate.Domain.Repositories;
using AirTicketSystem.modules.route.Domain.aggregate;
using AirTicketSystem.modules.route.Domain.Repositories;
using AirTicketSystem.modules.fare.Domain.aggregate;
using AirTicketSystem.modules.fare.Domain.Repositories;
using AirTicketSystem.modules.luggagerestriction.Domain.aggregate;
using AirTicketSystem.modules.luggagerestriction.Domain.Repositories;
using AirTicketSystem.modules.aircraft.Domain.aggregate;
using AirTicketSystem.modules.aircraft.Domain.Repositories;
using AirTicketSystem.modules.aircraftseat.Domain.aggregate;
using AirTicketSystem.modules.aircraftseat.Domain.Repositories;
using AirTicketSystem.modules.person.Domain.aggregate;
using AirTicketSystem.modules.person.Domain.Repositories;
using AirTicketSystem.modules.worker.Domain.aggregate;
using AirTicketSystem.modules.worker.Domain.Repositories;
using AirTicketSystem.modules.pilotlicense.Domain.aggregate;
using AirTicketSystem.modules.pilotlicense.Domain.Repositories;
using AirTicketSystem.modules.pilotrating.Domain.aggregate;
using AirTicketSystem.modules.pilotrating.Domain.Repositories;
using AirTicketSystem.modules.role.Domain.Repositories;
using AirTicketSystem.modules.user.Domain.aggregate;
using AirTicketSystem.modules.user.Domain.Repositories;
using AirTicketSystem.modules.client.Domain.aggregate;
using AirTicketSystem.modules.client.Domain.Repositories;
using AirTicketSystem.modules.flight.Domain.aggregate;
using AirTicketSystem.modules.flight.Domain.Repositories;
using AirTicketSystem.modules.flightcrew.Domain.aggregate;
using AirTicketSystem.modules.flightcrew.Domain.Repositories;
using AirTicketSystem.modules.seatavailability.Domain.aggregate;
using AirTicketSystem.modules.seatavailability.Domain.Repositories;
using AirTicketSystem.modules.booking.Domain.aggregate;
using AirTicketSystem.modules.booking.Domain.Repositories;
using AirTicketSystem.modules.bookingpassenger.Domain.aggregate;
using AirTicketSystem.modules.bookingpassenger.Domain.Repositories;
using AirTicketSystem.modules.luggage.Domain.aggregate;
using AirTicketSystem.modules.luggage.Domain.Repositories;
using AirTicketSystem.modules.payment.Domain.aggregate;
using AirTicketSystem.modules.payment.Domain.Repositories;
using AirTicketSystem.modules.additionalcharge.Domain.aggregate;
using AirTicketSystem.modules.additionalcharge.Domain.Repositories;
using AirTicketSystem.modules.checkin.Domain.aggregate;
using AirTicketSystem.modules.checkin.Domain.Repositories;
using AirTicketSystem.modules.ticket.Domain.aggregate;
using AirTicketSystem.modules.ticket.Domain.Repositories;
using AirTicketSystem.modules.boardingpass.Domain.aggregate;
using AirTicketSystem.modules.boardingpass.Domain.Repositories;
using AirTicketSystem.modules.invoice.Domain.aggregate;
using AirTicketSystem.modules.invoice.Domain.Repositories;

namespace AirTicketSystem.shared;

public sealed class SeedFullDataUseCase
{
    private readonly IServiceProvider _provider;

    public SeedFullDataUseCase(IServiceProvider provider) => _provider = provider;

    public async Task ExecuteAsync(CancellationToken ct = default)
    {
        await using var scope = _provider.CreateAsyncScope();
        var sp = scope.ServiceProvider;

        // Guard: si ya hay géneros, el seed ya corrió (géneros son lo primero que se inserta)
        var generoRepo = sp.GetRequiredService<IGenderRepository>();
        var generosExistentes = await generoRepo.FindAllAsync();
        if (generosExistentes.Count > 0) return;

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("  ► Ejecutando seed completo de datos de prueba...");
        Console.ResetColor();

        // Usa transacción con la estrategia de reintentos de MySQL
        var dbContext = sp.GetRequiredService<AppDbContext>();
        var strategy  = dbContext.Database.CreateExecutionStrategy();

        await strategy.ExecuteAsync(async () =>
        {
        await using var transaction = await dbContext.Database.BeginTransactionAsync(ct);

        static void Log(string paso) {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"    Seed paso: {paso}");
            Console.ResetColor();
        }

        // ── 1. CATÁLOGOS ─────────────────────────────────────────────────────
        Log("géneros");
        // Géneros
        var generoM = Gender.Crear("Masculino");
        var generoF = Gender.Crear("Femenino");
        var generoX = Gender.Crear("No especificado");
        var generoRepo = sp.GetRequiredService<IGenderRepository>();
        await generoRepo.SaveAsync(generoM);
        await generoRepo.SaveAsync(generoF);
        await generoRepo.SaveAsync(generoX);

        Log("tipos de documento");
        var docRepo       = sp.GetRequiredService<IDocumentTypeRepository>();
        var docsExist     = (await docRepo.FindAllAsync()).ToDictionary(d => d.Descripcion.Valor);

        async Task<DocumentType> ObtenerOCrearDoc(string desc)
        {
            if (docsExist.TryGetValue(desc, out var ex)) return ex;
            var nuevo = DocumentType.Crear(desc);
            await docRepo.SaveAsync(nuevo);
            return nuevo;
        }

        var docCC = await ObtenerOCrearDoc("Cédula de ciudadanía");
        var docPA = await ObtenerOCrearDoc("Pasaporte");
        var docCE = await ObtenerOCrearDoc("Cédula de extranjería");
        var docTI = await ObtenerOCrearDoc("Tarjeta de identidad");

        Log("tipos de teléfono");
        var telCel  = PhoneType.Crear("Celular");
        var telFijo = PhoneType.Crear("Fijo");
        var telTrab = PhoneType.Crear("Trabajo");
        var telEmg  = PhoneType.Crear("Emergencia");
        var telRepo = sp.GetRequiredService<IPhoneTypeRepository>();
        await telRepo.SaveAsync(telCel);
        await telRepo.SaveAsync(telFijo);
        await telRepo.SaveAsync(telTrab);
        await telRepo.SaveAsync(telEmg);

        Log("tipos de email");
        var emailPers = EmailType.Crear("Personal");
        var emailTrab = EmailType.Crear("Trabajo");
        var emailOtro = EmailType.Crear("Otro");
        var emailRepo = sp.GetRequiredService<IEmailTypeRepository>();
        await emailRepo.SaveAsync(emailPers);
        await emailRepo.SaveAsync(emailTrab);
        await emailRepo.SaveAsync(emailOtro);

        Log("tipos de dirección");
        var dirRes  = AddressType.Crear("Residencia");
        var dirTrab = AddressType.Crear("Trabajo");
        var dirCorr = AddressType.Crear("Correspondencia");
        var dirRepo = sp.GetRequiredService<IAddressTypeRepository>();
        await dirRepo.SaveAsync(dirRes);
        await dirRepo.SaveAsync(dirTrab);
        await dirRepo.SaveAsync(dirCorr);

        Log("relaciones de contacto");
        var relFam  = ContactRelationship.Crear("Familiar");
        var relCon  = ContactRelationship.Crear("Cónyuge");
        var relAmg  = ContactRelationship.Crear("Amigo/a");
        var relVec  = ContactRelationship.Crear("Vecino/a");
        var relRepo = sp.GetRequiredService<IContactRelationshipRepository>();
        await relRepo.SaveAsync(relFam);
        await relRepo.SaveAsync(relCon);
        await relRepo.SaveAsync(relAmg);
        await relRepo.SaveAsync(relVec);

        Log("tipos de equipaje");
        var eqMano    = LuggageType.Crear("Equipaje de mano");
        var eqBodSmall= LuggageType.Crear("Maleta bodega pequeña");
        var eqBodBig  = LuggageType.Crear("Maleta bodega grande");
        var eqEsp     = LuggageType.Crear("Equipaje especial");
        var eqRepo    = sp.GetRequiredService<ILuggageTypeRepository>();
        await eqRepo.SaveAsync(eqMano);
        await eqRepo.SaveAsync(eqBodSmall);
        await eqRepo.SaveAsync(eqBodBig);
        await eqRepo.SaveAsync(eqEsp);

        Log("clases de servicio");
        var claseY = ServiceClass.Crear("Económica",    "ECO", "Clase económica estándar");
        var claseW = ServiceClass.Crear("Económica Plus","EPL", "Económica con más espacio");
        var claseC = ServiceClass.Crear("Ejecutiva",    "EJE", "Clase ejecutiva");
        var claseF = ServiceClass.Crear("Primera Clase","PRC", "Primera clase premium");
        var claseRepo = sp.GetRequiredService<IServiceClassRepository>();
        await claseRepo.SaveAsync(claseY);
        await claseRepo.SaveAsync(claseW);
        await claseRepo.SaveAsync(claseC);
        await claseRepo.SaveAsync(claseF);

        Log("tipos de trabajador");
        var tipoPiloto    = WorkerType.Crear("Piloto");
        var tipoCabina    = WorkerType.Crear("Tripulación de cabina");
        var tipoTecnico   = WorkerType.Crear("Personal técnico");
        var tipoTierra    = WorkerType.Crear("Personal de tierra");
        var tipoAdmin     = WorkerType.Crear("Administrativo");
        var wtRepo        = sp.GetRequiredService<IWorkerTypeRepository>();
        await wtRepo.SaveAsync(tipoPiloto);
        await wtRepo.SaveAsync(tipoCabina);
        await wtRepo.SaveAsync(tipoTecnico);
        await wtRepo.SaveAsync(tipoTierra);
        await wtRepo.SaveAsync(tipoAdmin);

        Log("especialidades");
        var espCapitan  = Specialty.Crear("Capitán",                   tipoPiloto.Id);
        var espPrimOfi  = Specialty.Crear("Primer Oficial",             tipoPiloto.Id);
        var espAuxJefe  = Specialty.Crear("Auxiliar de vuelo jefe",    tipoCabina.Id);
        var espAux      = Specialty.Crear("Auxiliar de vuelo",         tipoCabina.Id);
        var espMecanico = Specialty.Crear("Mecánico aeronáutico",      tipoTecnico.Id);
        var espRepo     = sp.GetRequiredService<ISpecialtyRepository>();
        await espRepo.SaveAsync(espCapitan);
        await espRepo.SaveAsync(espPrimOfi);
        await espRepo.SaveAsync(espAuxJefe);
        await espRepo.SaveAsync(espAux);
        await espRepo.SaveAsync(espMecanico);

        Log("métodos de pago");
        var mpTC   = PaymentMethod.Crear("Tarjeta de crédito");
        var mpTD   = PaymentMethod.Crear("Tarjeta débito");
        var mpTrans= PaymentMethod.Crear("Transferencia bancaria");
        var mpPSE  = PaymentMethod.Crear("PSE");
        var mpEfec = PaymentMethod.Crear("Efectivo");
        var mpRepo = sp.GetRequiredService<IPaymentMethodRepository>();
        await mpRepo.SaveAsync(mpTC);
        await mpRepo.SaveAsync(mpTD);
        await mpRepo.SaveAsync(mpTrans);
        await mpRepo.SaveAsync(mpPSE);
        await mpRepo.SaveAsync(mpEfec);

        Log("geografía - continentes");

        var continRepo = sp.GetRequiredService<IContinentRepository>();
        var countRepo  = sp.GetRequiredService<ICountryRepository>();
        var regionRepo = sp.GetRequiredService<IRegionRepository>();
        var deptRepo   = sp.GetRequiredService<IDepartmentRepository>();
        var cityRepo   = sp.GetRequiredService<ICityRepository>();

        var contAm = Continent.Crear("América",  "AM");
        var contEu = Continent.Crear("Europa",   "EU");
        await continRepo.SaveAsync(contAm);
        await continRepo.SaveAsync(contEu);

        var paisCO = Country.Crear(contAm.Id, "Colombia", "CO", "COL");
        var paisES = Country.Crear(contEu.Id, "España",   "ES", "ESP");
        await countRepo.SaveAsync(paisCO);
        await countRepo.SaveAsync(paisES);

        var regCund  = Region.Crear(paisCO.Id, "Cundinamarca");
        var regAnt   = Region.Crear(paisCO.Id, "Antioquia");
        var regBol   = Region.Crear(paisCO.Id, "Bolívar");
        var regMad   = Region.Crear(paisES.Id, "Comunidad de Madrid");
        await regionRepo.SaveAsync(regCund);
        await regionRepo.SaveAsync(regAnt);
        await regionRepo.SaveAsync(regBol);
        await regionRepo.SaveAsync(regMad);

        var deptCund = Department.Crear(regCund.Id, "Cundinamarca");
        var deptAnt  = Department.Crear(regAnt.Id,  "Antioquia");
        var deptBol  = Department.Crear(regBol.Id,  "Bolívar");
        var deptMad  = Department.Crear(regMad.Id,  "Madrid");
        await deptRepo.SaveAsync(deptCund);
        await deptRepo.SaveAsync(deptAnt);
        await deptRepo.SaveAsync(deptBol);
        await deptRepo.SaveAsync(deptMad);

        var ciudadBog = City.Crear(deptCund.Id, "Bogotá");
        var ciudadMed = City.Crear(deptAnt.Id,  "Medellín");
        var ciudadCTG = City.Crear(deptBol.Id,  "Cartagena de Indias");
        var ciudadMad = City.Crear(deptMad.Id,  "Madrid");
        await cityRepo.SaveAsync(ciudadBog);
        await cityRepo.SaveAsync(ciudadMed);
        await cityRepo.SaveAsync(ciudadCTG);
        await cityRepo.SaveAsync(ciudadMad);

        Log("aerolíneas y flota");

        var manufRepo  = sp.GetRequiredService<IAircraftManufacturerRepository>();
        var modelRepo  = sp.GetRequiredService<IAircraftModelRepository>();
        var airlineRepo= sp.GetRequiredService<IAirlineRepository>();
        var airportRepo= sp.GetRequiredService<IAirportRepository>();
        var termRepo   = sp.GetRequiredService<ITerminalRepository>();
        var gateRepo   = sp.GetRequiredService<IGateRepository>();
        var routeRepo  = sp.GetRequiredService<IRouteRepository>();
        var fareRepo   = sp.GetRequiredService<IFareRepository>();
        var lrRepo     = sp.GetRequiredService<ILuggageRestrictionRepository>();
        var acRepo     = sp.GetRequiredService<IAircraftRepository>();
        var seatRepo   = sp.GetRequiredService<IAircraftSeatRepository>();

        var fabAirbus = AircraftManufacturer.Crear(paisES.Id, "Airbus", "https://www.airbus.com");
        var fabBoeing = AircraftManufacturer.Crear(paisCO.Id, "Boeing");
        await manufRepo.SaveAsync(fabAirbus);
        await manufRepo.SaveAsync(fabBoeing);

        var modelA320 = AircraftModel.Crear(fabAirbus.Id, "A320neo",  "A320neo", 6300, 833, "Familia A320, motor LEAP");
        var modelA319 = AircraftModel.Crear(fabAirbus.Id, "A319",     "A319",    6850, 840, "Variante corta del A320");
        var model737  = AircraftModel.Crear(fabBoeing.Id, "737-800",  "B738",    5765, 842, "Boeing 737 NG");
        await modelRepo.SaveAsync(modelA320);
        await modelRepo.SaveAsync(modelA319);
        await modelRepo.SaveAsync(model737);

        var avianca = Airline.Crear(paisCO.Id, "AV", "AVA", "Avianca S.A.", "Avianca", "https://www.avianca.com");
        var latam   = Airline.Crear(paisCO.Id, "LT", "LTC", "LATAM Airlines Colombia", "LATAM Colombia");
        await airlineRepo.SaveAsync(avianca);
        await airlineRepo.SaveAsync(latam);

        var airportBOG = Airport.Crear(ciudadBog.Id, "BOG", "SKBO", "Aeropuerto Internacional El Dorado",         "Av. El Dorado, Bogotá");
        var airportEOH = Airport.Crear(ciudadMed.Id, "EOH", "SKMD", "Aeropuerto Olaya Herrera",                   "Cra. 65A #13-157, Medellín");
        var airportCTG = Airport.Crear(ciudadCTG.Id, "CTG", "SKCG", "Aeropuerto Internacional Rafael Núñez",      "Cartagena de Indias");
        await airportRepo.SaveAsync(airportBOG);
        await airportRepo.SaveAsync(airportEOH);
        await airportRepo.SaveAsync(airportCTG);

        // Terminales
        var termBOG1 = Terminal.Crear(airportBOG.Id, "T1 Nacional",       "Vuelos nacionales");
        var termBOG2 = Terminal.Crear(airportBOG.Id, "T2 Internacional",   "Vuelos internacionales");
        var termEOH  = Terminal.Crear(airportEOH.Id, "Terminal Única",     "Terminal única");
        var termCTG  = Terminal.Crear(airportCTG.Id, "Terminal Principal", "Terminal única");
        await termRepo.SaveAsync(termBOG1);
        await termRepo.SaveAsync(termBOG2);
        await termRepo.SaveAsync(termEOH);
        await termRepo.SaveAsync(termCTG);

        // Puertas de embarque
        Gate[] puertas = [
            Gate.Crear(termBOG1.Id, "A1"), Gate.Crear(termBOG1.Id, "A2"), Gate.Crear(termBOG1.Id, "A3"),
            Gate.Crear(termBOG1.Id, "B1"), Gate.Crear(termBOG1.Id, "B2"),
            Gate.Crear(termBOG2.Id, "C1"), Gate.Crear(termBOG2.Id, "C2"),
            Gate.Crear(termEOH.Id,  "G1"), Gate.Crear(termEOH.Id,  "G2"),
            Gate.Crear(termCTG.Id,  "P1"), Gate.Crear(termCTG.Id,  "P2"),
        ];
        foreach (var g in puertas) await gateRepo.SaveAsync(g);
        var puertaA1 = puertas[0];  // BOG T1 A1
        var puertaG1 = puertas[7];  // EOH G1
        var puertaP1 = puertas[9];  // CTG P1

        // Rutas
        var rutaBOGMDE_AV = Route.Crear(avianca.Id, airportBOG.Id, airportEOH.Id, 235, 45);
        var rutaBOGCTG_AV = Route.Crear(avianca.Id, airportBOG.Id, airportCTG.Id, 875, 55);
        var rutaBOGMDE_LT = Route.Crear(latam.Id,   airportBOG.Id, airportEOH.Id, 235, 50);
        await routeRepo.SaveAsync(rutaBOGMDE_AV);
        await routeRepo.SaveAsync(rutaBOGCTG_AV);
        await routeRepo.SaveAsync(rutaBOGMDE_LT);

        var hoy2 = DateOnly.FromDateTime(DateTime.Today).AddYears(1);

        // Tarifas (por ruta)
        var tarifaBasicaAVMDE = Fare.Crear(rutaBOGMDE_AV.Id, claseY.Id, "Básica",    150_000, 27_000, false, false, hoy2);
        var tarifaFlex_AVMDE  = Fare.Crear(rutaBOGMDE_AV.Id, claseY.Id, "Flexible",  210_000, 37_800, true,  false, hoy2);
        var tarifaEjec_AVMDE  = Fare.Crear(rutaBOGMDE_AV.Id, claseC.Id, "Ejecutiva", 480_000, 86_400, true,  true,  hoy2);

        var tarifaBasicaAVCTG = Fare.Crear(rutaBOGCTG_AV.Id, claseY.Id, "Básica",    180_000, 32_400, false, false, hoy2);
        var tarifaFlex_AVCTG  = Fare.Crear(rutaBOGCTG_AV.Id, claseY.Id, "Flexible",  240_000, 43_200, true,  false, hoy2);

        var tarifaBasicaLTMDE = Fare.Crear(rutaBOGMDE_LT.Id, claseY.Id, "Básica",    145_000, 26_100, false, false, hoy2);
        var tarifaFlex_LTMDE  = Fare.Crear(rutaBOGMDE_LT.Id, claseY.Id, "Flexible",  200_000, 36_000, true,  false, hoy2);

        foreach (var f in new[]{ tarifaBasicaAVMDE, tarifaFlex_AVMDE, tarifaEjec_AVMDE,
                                  tarifaBasicaAVCTG, tarifaFlex_AVCTG,
                                  tarifaBasicaLTMDE, tarifaFlex_LTMDE })
            await fareRepo.SaveAsync(f);

        // Restricciones de equipaje por tarifa
        var restBasica1 = LuggageRestriction.Crear(tarifaBasicaAVMDE.Id, eqMano.Id,     1, 10m, 0m, 55, 40, 25);
        var restBasica2 = LuggageRestriction.Crear(tarifaFlex_AVMDE.Id,  eqMano.Id,     1, 10m, 0m, 55, 40, 25);
        var restFlex1   = LuggageRestriction.Crear(tarifaFlex_AVMDE.Id,  eqBodSmall.Id, 1, 23m, 30_000m);
        var restEjec1   = LuggageRestriction.Crear(tarifaEjec_AVMDE.Id,  eqMano.Id,     1, 10m, 0m);
        var restEjec2   = LuggageRestriction.Crear(tarifaEjec_AVMDE.Id,  eqBodBig.Id,   2, 32m, 20_000m);
        foreach (var r in new[]{ restBasica1, restBasica2, restFlex1, restEjec1, restEjec2 })
            await lrRepo.SaveAsync(r);

        // Aviones
        var avionHK4800 = Aircraft.Crear(modelA320.Id, avianca.Id, "HK-4800",
            DateOnly.Parse("2018-03-15"), DateOnly.FromDateTime(DateTime.Today).AddMonths(6));
        var avionHK4801 = Aircraft.Crear(modelA319.Id, avianca.Id, "HK-4801",
            DateOnly.Parse("2019-07-20"), DateOnly.FromDateTime(DateTime.Today).AddMonths(8));
        var avionHK5010 = Aircraft.Crear(model737.Id,  latam.Id,   "HK-5010",
            DateOnly.Parse("2020-01-10"), DateOnly.FromDateTime(DateTime.Today).AddMonths(4));
        await acRepo.SaveAsync(avionHK4800);
        await acRepo.SaveAsync(avionHK4801);
        await acRepo.SaveAsync(avionHK5010);

        // Asientos (simplificados: 2 filas ejecutiva + 8 filas económica por avión)
        var asientosHK4800 = await SeedAsientosAsync(seatRepo, avionHK4800.Id, claseC.Id, claseY.Id);
        var asientosHK4801 = await SeedAsientosAsync(seatRepo, avionHK4801.Id, claseC.Id, claseY.Id);
        var asientosHK5010 = await SeedAsientosAsync(seatRepo, avionHK5010.Id, claseC.Id, claseY.Id);

        Log("personal - trabajadores");

        var personaRepo = sp.GetRequiredService<IPersonRepository>();
        var workerRepo  = sp.GetRequiredService<IWorkerRepository>();
        var licRepo     = sp.GetRequiredService<IPilotLicenseRepository>();
        var ratingRepo  = sp.GetRequiredService<IPilotRatingRepository>();

        // Personas para trabajadores
        var perCarlos  = Person.Crear(docCC.Id, "12345678",   "Carlos Alberto", "Mendoza Ríos",    DateOnly.Parse("1982-04-15"), generoM.Id, paisCO.Id);
        var perAna     = Person.Crear(docCC.Id, "87654321",   "Ana María",      "Rodríguez Vega",  DateOnly.Parse("1985-09-22"), generoF.Id, paisCO.Id);
        var perJorge   = Person.Crear(docCC.Id, "23456789",   "Jorge Andrés",   "Pérez Mora",      DateOnly.Parse("1990-11-08"), generoM.Id, paisCO.Id);
        var perMariaW  = Person.Crear(docCC.Id, "34567890",   "María Camila",   "López Castro",    DateOnly.Parse("1993-02-28"), generoF.Id, paisCO.Id);
        foreach (var p in new[]{ perCarlos, perAna, perJorge, perMariaW })
            await personaRepo.SaveAsync(p);

        // Trabajadores
        var trabCarlos = Worker.Crear(perCarlos.Id,  tipoPiloto.Id, airportBOG.Id,
            DateOnly.Parse("2010-06-01"), 8_500_000m, avianca.Id);
        var trabAna    = Worker.Crear(perAna.Id,     tipoPiloto.Id, airportBOG.Id,
            DateOnly.Parse("2014-03-15"), 7_800_000m, latam.Id);
        var trabJorge  = Worker.Crear(perJorge.Id,   tipoCabina.Id, airportBOG.Id,
            DateOnly.Parse("2016-08-01"), 2_800_000m, avianca.Id);
        var trabMaria  = Worker.Crear(perMariaW.Id,  tipoCabina.Id, airportBOG.Id,
            DateOnly.Parse("2018-01-20"), 2_600_000m, latam.Id);
        await workerRepo.SaveAsync(trabCarlos);
        await workerRepo.SaveAsync(trabAna);
        await workerRepo.SaveAsync(trabJorge);
        await workerRepo.SaveAsync(trabMaria);

        // Licencias de piloto
        var licCarlos = PilotLicense.Crear(trabCarlos.Id, "ATPL-CO-001234", "ATPL",
            DateOnly.Parse("2010-05-01"), DateOnly.Parse("2027-05-01"), "Aerocivil Colombia");
        var licAna    = PilotLicense.Crear(trabAna.Id,    "CPL-CO-005678",  "CPL",
            DateOnly.Parse("2014-02-01"), DateOnly.Parse("2026-02-01"), "Aerocivil Colombia");
        await licRepo.SaveAsync(licCarlos);
        await licRepo.SaveAsync(licAna);

        // Habilitaciones (rating) por modelo de avión
        var ratCarlosA320 = PilotRating.Crear(licCarlos.Id, modelA320.Id,
            DateOnly.Parse("2018-01-01"), DateOnly.Parse("2027-01-01"));
        var ratAna737     = PilotRating.Crear(licAna.Id,    model737.Id,
            DateOnly.Parse("2020-06-01"), DateOnly.Parse("2026-06-01"));
        await ratingRepo.SaveAsync(ratCarlosA320);
        await ratingRepo.SaveAsync(ratAna737);

        Log("usuarios y clientes");

        var roleRepo   = sp.GetRequiredService<IRoleRepository>();
        var userRepo   = sp.GetRequiredService<IUserRepository>();
        var clientRepo = sp.GetRequiredService<IClientRepository>();
        var phoneRepo  = sp.GetRequiredService<IPersonPhoneRepository>();
        var emailSRepo = sp.GetRequiredService<IPersonEmailRepository>();
        var addrRepo   = sp.GetRequiredService<IPersonAddressRepository>();
        var ecRepo     = sp.GetRequiredService<IEmergencyContactRepository>();

        var allRoles   = await roleRepo.FindAllAsync();
        var rolCliente = allRoles.First(r => r.Nombre.Valor == "CLIENTE");

        // Personas para clientes
        var perJuan  = Person.Crear(docCC.Id, "11223344", "Juan Carlos",   "Herrera Botero",  DateOnly.Parse("1995-07-14"), generoM.Id, paisCO.Id);
        var perLaura = Person.Crear(docCC.Id, "44332211", "Laura Patricia","García Salcedo",  DateOnly.Parse("1998-03-25"), generoF.Id, paisCO.Id);
        await personaRepo.SaveAsync(perJuan);
        await personaRepo.SaveAsync(perLaura);

        // Teléfonos para clientes
        var telJuan  = PersonPhone.Crear(perJuan.Id,  telCel.Id, "3101234567", "+57", true);
        var telLaura = PersonPhone.Crear(perLaura.Id, telCel.Id, "3209876543", "+57", true);
        await phoneRepo.SaveAsync(telJuan);
        await phoneRepo.SaveAsync(telLaura);

        // Emails para clientes
        var emailJuan  = PersonEmail.Crear(perJuan.Id,  emailPers.Id, "juanherrera@email.com",  true);
        var emailLaura = PersonEmail.Crear(perLaura.Id, emailPers.Id, "lauragarcia@email.com",  true);
        await emailSRepo.SaveAsync(emailJuan);
        await emailSRepo.SaveAsync(emailLaura);

        // Direcciones para clientes (necesarias para la factura)
        var addrJuan  = PersonAddress.Crear(perJuan.Id,  dirRes.Id, ciudadBog.Id, "Cra 15 # 85-32 Apto 201", null, null, true);
        var addrLaura = PersonAddress.Crear(perLaura.Id, dirRes.Id, ciudadMed.Id, "Cl 30 # 45-10 Casa 5",    null, null, true);
        await addrRepo.SaveAsync(addrJuan);
        await addrRepo.SaveAsync(addrLaura);

        // Personas para contactos de emergencia
        var perEcJuan  = Person.Crear(docCC.Id, "55667788", "Pedro José",   "Herrera López",   DateOnly.Parse("1968-05-10"), generoM.Id, paisCO.Id);
        var perEcLaura = Person.Crear(docCC.Id, "88776655", "Rosa Elena",   "Salcedo Torres",  DateOnly.Parse("1971-11-30"), generoF.Id, paisCO.Id);
        await personaRepo.SaveAsync(perEcJuan);
        await personaRepo.SaveAsync(perEcLaura);

        // Usuarios clientes
        var hashCli   = PasswordHasher.Hash("cliente123");
        var userJuan  = User.Crear(perJuan.Id,  rolCliente.Id, "juanherrera",  hashCli);
        var userLaura = User.Crear(perLaura.Id, rolCliente.Id, "lauragarcia",  hashCli);
        await userRepo.SaveAsync(userJuan);
        await userRepo.SaveAsync(userLaura);

        // Clientes
        var clienteJuan  = Client.Crear(perJuan.Id,  userJuan.Id);
        var clienteLaura = Client.Crear(perLaura.Id, userLaura.Id);
        await clientRepo.SaveAsync(clienteJuan);
        await clientRepo.SaveAsync(clienteLaura);

        // Contactos de emergencia
        var ecJuan  = EmergencyContact.Crear(clienteJuan.Id,  perEcJuan.Id,  relFam.Id, true);
        var ecLaura = EmergencyContact.Crear(clienteLaura.Id, perEcLaura.Id, relFam.Id, true);
        await ecRepo.SaveAsync(ecJuan);
        await ecRepo.SaveAsync(ecLaura);

        Log("vuelos");

        var flightRepo  = sp.GetRequiredService<IFlightRepository>();
        var crewRepo    = sp.GetRequiredService<IFlightCrewRepository>();
        var availRepo   = sp.GetRequiredService<ISeatAvailabilityRepository>();

        var base3d  = DateTime.UtcNow.AddDays(3);
        var base5d  = DateTime.UtcNow.AddDays(5);
        var base4d  = DateTime.UtcNow.AddDays(4);

        // Vuelo 1: Avianca BOG→MDE en 3 días
        var vuelo1 = Flight.Crear(rutaBOGMDE_AV.Id, avionHK4800.Id, "AV0101",
            new DateTime(base3d.Year, base3d.Month, base3d.Day, 6, 0, 0, DateTimeKind.Utc),
            new DateTime(base3d.Year, base3d.Month, base3d.Day, 6, 45, 0, DateTimeKind.Utc),
            puertaA1.Id);
        // Vuelo 2: Avianca BOG→CTG en 5 días
        var vuelo2 = Flight.Crear(rutaBOGCTG_AV.Id, avionHK4800.Id, "AV0202",
            new DateTime(base5d.Year, base5d.Month, base5d.Day, 8, 0, 0, DateTimeKind.Utc),
            new DateTime(base5d.Year, base5d.Month, base5d.Day, 8, 55, 0, DateTimeKind.Utc),
            puertaA1.Id);
        // Vuelo 3: LATAM BOG→MDE en 4 días
        var vuelo3 = Flight.Crear(rutaBOGMDE_LT.Id, avionHK5010.Id, "LT0303",
            new DateTime(base4d.Year, base4d.Month, base4d.Day, 10, 0, 0, DateTimeKind.Utc),
            new DateTime(base4d.Year, base4d.Month, base4d.Day, 10, 50, 0, DateTimeKind.Utc),
            puertaG1.Id);
        await flightRepo.SaveAsync(vuelo1);
        await flightRepo.SaveAsync(vuelo2);
        await flightRepo.SaveAsync(vuelo3);

        // Tripulación de vuelos
        var crew_v1_carlos = FlightCrew.Crear(vuelo1.Id, trabCarlos.Id, "PILOTO");
        var crew_v1_jorge  = FlightCrew.Crear(vuelo1.Id, trabJorge.Id,  "AUXILIAR_VUELO");
        var crew_v2_carlos = FlightCrew.Crear(vuelo2.Id, trabCarlos.Id, "PILOTO");
        var crew_v2_jorge  = FlightCrew.Crear(vuelo2.Id, trabJorge.Id,  "AUXILIAR_VUELO");
        var crew_v3_ana    = FlightCrew.Crear(vuelo3.Id, trabAna.Id,    "PILOTO");
        var crew_v3_maria  = FlightCrew.Crear(vuelo3.Id, trabMaria.Id,  "AUXILIAR_VUELO");
        foreach (var c in new[]{ crew_v1_carlos, crew_v1_jorge, crew_v2_carlos,
                                  crew_v2_jorge, crew_v3_ana, crew_v3_maria })
            await crewRepo.SaveAsync(c);

        // Disponibilidad de asientos
        var dispV1 = asientosHK4800.Select(a => SeatAvailability.Crear(vuelo1.Id, a.Id)).ToList();
        var dispV2 = asientosHK4800.Select(a => SeatAvailability.Crear(vuelo2.Id, a.Id)).ToList();
        var dispV3 = asientosHK5010.Select(a => SeatAvailability.Crear(vuelo3.Id, a.Id)).ToList();
        await availRepo.SaveAllAsync(dispV1);
        await availRepo.SaveAllAsync(dispV2);
        await availRepo.SaveAllAsync(dispV3);

        Log("reservas, pagos, tiquetes");

        var bookingRepo  = sp.GetRequiredService<IBookingRepository>();
        var paxRepo      = sp.GetRequiredService<IBookingPassengerRepository>();
        var luggageRepo  = sp.GetRequiredService<ILuggageRepository>();
        var paymentRepo  = sp.GetRequiredService<IPaymentRepository>();
        var chargeRepo   = sp.GetRequiredService<IAdditionalChargeRepository>();
        var ticketRepo   = sp.GetRequiredService<ITicketRepository>();
        var checkinRepo  = sp.GetRequiredService<ICheckInRepository>();
        var bpRepo       = sp.GetRequiredService<IBoardingPassRepository>();
        var invoiceRepo  = sp.GetRequiredService<IInvoiceRepository>();

        // Asiento económico fila 5A en HK-4800 (para la reserva de Juan)
        var asientoJuan = asientosHK4800.First(a => a.ClaseServicioId == claseY.Id);

        // Reserva 1 — Juan en AV0101 (Básica) → CONFIRMADA
        var reservaJuan = Booking.Crear(clienteJuan.Id, vuelo1.Id, tarifaBasicaAVMDE.Id,
            tarifaBasicaAVMDE.PrecioTotal.Valor, "Viaje de negocios");
        reservaJuan.Confirmar();
        await bookingRepo.SaveAsync(reservaJuan);

        // Reserva 2 — Laura en LT0303 (Flexible) → PENDIENTE
        var reservaLaura = Booking.Crear(clienteLaura.Id, vuelo3.Id, tarifaFlex_LTMDE.Id,
            tarifaFlex_LTMDE.PrecioTotal.Valor);
        await bookingRepo.SaveAsync(reservaLaura);

        // Pasajeros
        var paxJuan  = BookingPassenger.CrearAdulto(reservaJuan.Id,  perJuan.Id,  asientoJuan.Id);
        var paxLaura = BookingPassenger.CrearAdulto(reservaLaura.Id, perLaura.Id, null);
        await paxRepo.SaveAsync(paxJuan);
        await paxRepo.SaveAsync(paxLaura);

        // Reservar asiento de Juan en disponibilidad
        var dispJuan = dispV1.First(d => d.AsientoId == asientoJuan.Id);
        dispJuan.Reservar();
        await availRepo.UpdateAsync(dispJuan);

        // Equipaje de Juan
        var equipJuan = Luggage.Crear(paxJuan.Id, vuelo1.Id, eqBodSmall.Id, "Maleta ejecutiva negra", 18m);
        await luggageRepo.SaveAsync(equipJuan);

        // Pago de Juan (aprobado)
        var pagoJuan = Payment.Crear(reservaJuan.Id, mpTC.Id, tarifaBasicaAVMDE.PrecioTotal.Valor);
        pagoJuan.Aprobar("TXN-BOG-2024-001");
        await paymentRepo.SaveAsync(pagoJuan);

        // Cargo adicional (selección de asiento)
        var cargoJuan = AdditionalCharge.Crear(reservaJuan.Id, "Selección de asiento ventana", 12_000m);
        await chargeRepo.SaveAsync(cargoJuan);

        // Tiquete de Juan
        var tiqueteJuan = Ticket.Crear(paxJuan.Id, asientoJuan.Id);
        await ticketRepo.SaveAsync(tiqueteJuan);

        // Check-in virtual de Juan
        var checkinJuan = CheckIn.CrearVirtual(paxJuan.Id);
        checkinJuan.Completar();
        await checkinRepo.SaveAsync(checkinJuan);

        // Pase de abordar de Juan
        var paseJuan = BoardingPass.Crear(
            checkinJuan.Id,
            "AV0101",
            $"{asientoJuan.Fila.Valor}{asientoJuan.Columna.Valor}",
            vuelo1.FechaSalida.Valor,
            puertaA1.Id,
            vuelo1.FechaSalida.Valor.AddMinutes(-30));
        await bpRepo.SaveAsync(paseJuan);

        // Factura de Juan
        var facturaJuan = Invoice.Crear(
            reservaJuan.Id,
            addrJuan.Id,
            tarifaBasicaAVMDE.PrecioBase.Valor,
            19m);
        await invoiceRepo.SaveAsync(facturaJuan);

        await transaction.CommitAsync(ct);

        }); // fin strategy.ExecuteAsync

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("  ✓ Seed completo ejecutado correctamente.");
        Console.ResetColor();
    }

    // ── Helper: crea asientos para un avión ──────────────────────────────────
    private static async Task<List<AircraftSeat>> SeedAsientosAsync(
        IAircraftSeatRepository repo,
        int avionId,
        int claseEjec,
        int claseEco)
    {
        var lista = new List<AircraftSeat>();

        // 2 filas ejecutiva: A,B,C
        for (int fila = 1; fila <= 2; fila++)
            foreach (char col in new[]{ 'A','B','C' })
            {
                var s = AircraftSeat.Crear(avionId, claseEjec, fila, col,
                    col == 'A' || col == 'C', col == 'B');
                await repo.SaveAsync(s);
                lista.Add(s);
            }

        // 8 filas económica: A,B,C,D,E,F
        for (int fila = 10; fila <= 17; fila++)
            foreach (char col in new[]{ 'A','B','C','D','E','F' })
            {
                var s = AircraftSeat.Crear(avionId, claseEco, fila, col,
                    col == 'A' || col == 'F', col == 'C' || col == 'D');
                await repo.SaveAsync(s);
                lista.Add(s);
            }

        return lista;
    }
}
