// src/modules/flight/Application/Services/FlightService.cs
using AirTicketSystem.modules.flight.Application.Interfaces;
using AirTicketSystem.modules.flight.Application.UseCases;
using AirTicketSystem.modules.flight.Domain.aggregate;

namespace AirTicketSystem.modules.flight.Application.Services;

public sealed class FlightService : IFlightService
{
    private readonly CreateFlightUseCase _create;
    private readonly GetFlightByIdUseCase _getById;
    private readonly GetFlightByNumberAndDateUseCase _getByNumberAndDate;
    private readonly GetAllFlightsUseCase _getAll;
    private readonly GetFlightsByDateUseCase _getByDate;
    private readonly GetFlightsByRouteUseCase _getByRoute;
    private readonly GetScheduledFlightsUseCase _getScheduled;
    private readonly SearchFlightsUseCase _search;
    private readonly GetFlightsWithOpenCheckinUseCase _getOpenCheckin;
    private readonly UpdateFlightUseCase _update;
    private readonly OpenCheckinUseCase _openCheckin;
    private readonly AssignGateToFlightUseCase _assignGate;
    private readonly StartBoardingUseCase _startBoarding;
    private readonly StartFlightUseCase _startFlight;
    private readonly RegisterLandingFlightUseCase _registerLanding;
    private readonly CancelFlightUseCase _cancel;
    private readonly DelayFlightUseCase _delay;
    private readonly DivertFlightUseCase _divert;
    private readonly DeleteFlightUseCase _delete;

    public FlightService(
        CreateFlightUseCase create,
        GetFlightByIdUseCase getById,
        GetFlightByNumberAndDateUseCase getByNumberAndDate,
        GetAllFlightsUseCase getAll,
        GetFlightsByDateUseCase getByDate,
        GetFlightsByRouteUseCase getByRoute,
        GetScheduledFlightsUseCase getScheduled,
        SearchFlightsUseCase search,
        GetFlightsWithOpenCheckinUseCase getOpenCheckin,
        UpdateFlightUseCase update,
        OpenCheckinUseCase openCheckin,
        AssignGateToFlightUseCase assignGate,
        StartBoardingUseCase startBoarding,
        StartFlightUseCase startFlight,
        RegisterLandingFlightUseCase registerLanding,
        CancelFlightUseCase cancel,
        DelayFlightUseCase delay,
        DivertFlightUseCase divert,
        DeleteFlightUseCase delete)
    {
        _create            = create;
        _getById           = getById;
        _getByNumberAndDate = getByNumberAndDate;
        _getAll            = getAll;
        _getByDate         = getByDate;
        _getByRoute        = getByRoute;
        _getScheduled      = getScheduled;
        _search            = search;
        _getOpenCheckin    = getOpenCheckin;
        _update            = update;
        _openCheckin       = openCheckin;
        _assignGate        = assignGate;
        _startBoarding     = startBoarding;
        _startFlight       = startFlight;
        _registerLanding   = registerLanding;
        _cancel            = cancel;
        _delay             = delay;
        _divert            = divert;
        _delete            = delete;
    }

    public Task<Flight> CreateAsync(
        int rutaId, int avionId, string numeroVuelo,
        DateTime fechaSalida, DateTime fechaLlegadaEstimada,
        int? puertaEmbarqueId)
        => _create.ExecuteAsync(
            rutaId, avionId, numeroVuelo,
            fechaSalida, fechaLlegadaEstimada, puertaEmbarqueId);

    public Task<Flight> GetByIdAsync(int id)
        => _getById.ExecuteAsync(id);

    public Task<Flight> GetByNumberAndDateAsync(string numeroVuelo, DateTime fecha)
        => _getByNumberAndDate.ExecuteAsync(numeroVuelo, fecha);

    public Task<IReadOnlyCollection<Flight>> GetAllAsync()
        => _getAll.ExecuteAsync();

    public Task<IReadOnlyCollection<Flight>> GetByDateAsync(DateTime fecha)
        => _getByDate.ExecuteAsync(fecha);

    public Task<IReadOnlyCollection<Flight>> GetByRouteAsync(int rutaId)
        => _getByRoute.ExecuteAsync(rutaId);

    public Task<IReadOnlyCollection<Flight>> GetScheduledAsync()
        => _getScheduled.ExecuteAsync();

    public Task<IReadOnlyCollection<Flight>> SearchAsync(
        int origenId, int destinoId, DateTime fecha)
        => _search.ExecuteAsync(origenId, destinoId, fecha);

    public Task<IReadOnlyCollection<Flight>> GetWithOpenCheckinAsync()
        => _getOpenCheckin.ExecuteAsync();

    public Task<Flight> UpdateAsync(
        int id, DateTime fechaSalida,
        DateTime fechaLlegadaEstimada, int? puertaEmbarqueId)
        => _update.ExecuteAsync(id, fechaSalida, fechaLlegadaEstimada, puertaEmbarqueId);

    public Task OpenCheckinAsync(int id, DateTime apertura, DateTime cierre)
        => _openCheckin.ExecuteAsync(id, apertura, cierre);

    public Task AssignGateAsync(int id, int puertaEmbarqueId)
        => _assignGate.ExecuteAsync(id, puertaEmbarqueId);

    public Task StartBoardingAsync(int id) => _startBoarding.ExecuteAsync(id);
    public Task StartFlightAsync(int id) => _startFlight.ExecuteAsync(id);

    public Task RegisterLandingAsync(int id, DateTime fechaLlegadaReal)
        => _registerLanding.ExecuteAsync(id, fechaLlegadaReal);

    public Task CancelAsync(int id, string motivo) => _cancel.ExecuteAsync(id, motivo);
    public Task DelayAsync(int id, string motivo) => _delay.ExecuteAsync(id, motivo);
    public Task DivertAsync(int id, string motivo) => _divert.ExecuteAsync(id, motivo);
    public Task DeleteAsync(int id) => _delete.ExecuteAsync(id);
}