// src/modules/luggage/Application/UseCases/RegisterLuggageUseCase.cs
using AirTicketSystem.modules.luggage.Domain.aggregate;
using AirTicketSystem.modules.luggage.Domain.Repositories;
using AirTicketSystem.modules.bookingpassenger.Domain.Repositories;
using AirTicketSystem.modules.flight.Domain.Repositories;
using AirTicketSystem.modules.luggagetype.Domain.Repositories;

namespace AirTicketSystem.modules.luggage.Application.UseCases;

public sealed class RegisterLuggageUseCase
{
    private readonly ILuggageRepository          _luggageRepository;
    private readonly IBookingPassengerRepository _passengerRepository;
    private readonly IFlightRepository           _flightRepository;
    private readonly ILuggageTypeRepository      _luggageTypeRepository;

    public RegisterLuggageUseCase(
        ILuggageRepository          luggageRepository,
        IBookingPassengerRepository passengerRepository,
        IFlightRepository           flightRepository,
        ILuggageTypeRepository      luggageTypeRepository)
    {
        _luggageRepository     = luggageRepository;
        _passengerRepository   = passengerRepository;
        _flightRepository      = flightRepository;
        _luggageTypeRepository = luggageTypeRepository;
    }

    public async Task<Luggage> ExecuteAsync(
        int pasajeroReservaId,
        int vueloId,
        int tipoEquipajeId,
        string? descripcion = null,
        decimal? pesoDeclaradoKg = null,
        int? largoDeclaradoCm = null,
        int? anchoDeclaradoCm = null,
        int? altoDeclaradoCm = null,
        CancellationToken cancellationToken = default)
    {
        _ = await _passengerRepository.FindByIdAsync(pasajeroReservaId)
            ?? throw new KeyNotFoundException(
                $"No se encontró un pasajero de reserva con ID {pasajeroReservaId}.");

        _ = await _flightRepository.FindByIdAsync(vueloId)
            ?? throw new KeyNotFoundException(
                $"No se encontró un vuelo con ID {vueloId}.");

        _ = await _luggageTypeRepository.FindByIdAsync(tipoEquipajeId)
            ?? throw new KeyNotFoundException(
                $"No se encontró un tipo de equipaje con ID {tipoEquipajeId}.");

        var luggage = Luggage.Crear(
            pasajeroReservaId,
            vueloId,
            tipoEquipajeId,
            descripcion,
            pesoDeclaradoKg,
            largoDeclaradoCm,
            anchoDeclaradoCm,
            altoDeclaradoCm);

        await _luggageRepository.SaveAsync(luggage);
        return luggage;
    }
}
