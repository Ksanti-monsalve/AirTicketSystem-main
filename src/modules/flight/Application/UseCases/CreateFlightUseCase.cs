// src/modules/flight/Application/UseCases/CreateFlightUseCase.cs
using AirTicketSystem.modules.flight.Domain.Repositories;
using AirTicketSystem.modules.flight.Domain.aggregate;
using AirTicketSystem.modules.route.Domain.Repositories;
using AirTicketSystem.modules.aircraft.Domain.Repositories;
using AirTicketSystem.modules.aircraftseat.Domain.Repositories;
using AirTicketSystem.modules.gate.Domain.Repositories;
using AirTicketSystem.modules.seatavailability.Domain.Repositories;
using AirTicketSystem.modules.seatavailability.Domain.aggregate;
using AirTicketSystem.modules.seat.Domain.Repositories;
using AirTicketSystem.modules.seat.Infrastructure.entity;
using AirTicketSystem.shared.context;
using Microsoft.EntityFrameworkCore;

namespace AirTicketSystem.modules.flight.Application.UseCases;

public sealed class CreateFlightUseCase
{
    private readonly IFlightRepository          _flightRepository;
    private readonly IRouteRepository           _routeRepository;
    private readonly IAircraftRepository        _aircraftRepository;
    private readonly IAircraftSeatRepository    _seatRepository;
    private readonly IGateRepository            _gateRepository;
    private readonly ISeatAvailabilityRepository _availabilityRepository;
    private readonly ISeatRepository             _seatExamRepository;
    private readonly AppDbContext                _db;

    public CreateFlightUseCase(
        IFlightRepository          flightRepository,
        IRouteRepository           routeRepository,
        IAircraftRepository        aircraftRepository,
        IAircraftSeatRepository    seatRepository,
        IGateRepository            gateRepository,
        ISeatAvailabilityRepository availabilityRepository,
        ISeatRepository             seatExamRepository,
        AppDbContext                db)
    {
        _flightRepository       = flightRepository;
        _routeRepository        = routeRepository;
        _aircraftRepository     = aircraftRepository;
        _seatRepository         = seatRepository;
        _gateRepository         = gateRepository;
        _availabilityRepository = availabilityRepository;
        _seatExamRepository     = seatExamRepository;
        _db                     = db;
    }

    public async Task<Flight> ExecuteAsync(
        int rutaId,
        int avionId,
        string numeroVuelo,
        DateTime fechaSalida,
        DateTime fechaLlegadaEstimada,
        int? puertaEmbarqueId,
        CancellationToken cancellationToken = default)
    {
        var ruta = await _routeRepository.FindByIdAsync(rutaId)
            ?? throw new KeyNotFoundException(
                $"No se encontró una ruta con ID {rutaId}.");

        if (!ruta.Activa.Valor)
            throw new InvalidOperationException(
                "La ruta seleccionada está inactiva.");

        var avion = await _aircraftRepository.FindByIdAsync(avionId)
            ?? throw new KeyNotFoundException(
                $"No se encontró un avión con ID {avionId}.");

        if (!avion.Activo.Valor)
            throw new InvalidOperationException(
                $"El avión '{avion.Matricula.Valor}' está dado de baja.");

        if (avion.Estado.Valor != "DISPONIBLE")
            throw new InvalidOperationException(
                $"El avión '{avion.Matricula.Valor}' no está disponible. " +
                $"Estado actual: '{avion.Estado}'.");

        if (await _flightRepository.ExistsByNumeroVueloAndFechaAsync(
            numeroVuelo, fechaSalida))
            throw new InvalidOperationException(
                $"Ya existe un vuelo con número '{numeroVuelo}' " +
                "programado para esa fecha.");

        if (puertaEmbarqueId.HasValue)
        {
            var puerta = await _gateRepository.FindByIdAsync(puertaEmbarqueId.Value)
                ?? throw new KeyNotFoundException(
                    $"No se encontró una puerta con ID {puertaEmbarqueId.Value}.");

            if (!puerta.Activa.Valor)
                throw new InvalidOperationException(
                    $"La puerta '{puerta.Codigo.Valor}' está inactiva.");
        }

        var flight = Flight.Crear(
            rutaId, avionId, numeroVuelo,
            fechaSalida, fechaLlegadaEstimada, puertaEmbarqueId);

        await _flightRepository.SaveAsync(flight);

        // Generar disponibilidad de asientos automáticamente
        var asientos = await _seatRepository.FindByAvionAsync(avionId);

        var disponibilidades = asientos
            .Select(a => SeatAvailability.Crear(
                vueloId: flight.Id,
                asientoId: a.Id,
                numeroAsiento: a.CodigoAsiento.Valor,
                claseVueloId: a.ClaseServicioId))
            .ToList();

        await _availabilityRepository.SaveAllAsync(disponibilidades);

        // EXAMEN (literal): generar Seats (tabla "seats") por vuelo
        // Map: clases_servicio -> flight_classes
        var flightClasses = await _db.FlightClasses
            .AsNoTracking()
            .ToDictionaryAsync(fc => fc.Code);

        int MapServiceClassToFlightClassId(string serviceClassCode)
        {
            var fcCode = serviceClassCode switch
            {
                "ECO" => "ECO",
                "EJE" => "BUS",
                "PRC" => "FST",
                _     => "ECO"
            };

            if (!flightClasses.TryGetValue(fcCode, out var fc))
                throw new InvalidOperationException(
                    $"No existe FlightClass con code '{fcCode}'. Ejecute migraciones/seed.");

            return fc.Id;
        }

        // Cargar los códigos de clase de los asientos del avión
        var asientosFull = await _db.AsientosAvion
            .AsNoTracking()
            .Include(a => a.ClaseServicio)
            .Where(a => a.AvionId == avionId)
            .ToListAsync();

        var seats = asientosFull.Select(a => new SeatEntity
        {
            FlightId = flight.Id,
            SeatNumber = a.CodigoAsiento,
            FlightClassId = MapServiceClassToFlightClassId(a.ClaseServicio.Codigo),
            Status = "Available"
        }).ToList();

        await _seatExamRepository.SaveAllAsync(seats);

        return flight;
    }
}
