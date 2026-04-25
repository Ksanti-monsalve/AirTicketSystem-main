// src/modules/booking/Application/UseCases/CreateBookingUseCase.cs
using AirTicketSystem.modules.booking.Domain.aggregate;
using AirTicketSystem.modules.booking.Domain.Repositories;
using AirTicketSystem.modules.client.Domain.Repositories;
using AirTicketSystem.modules.flight.Domain.Repositories;
using AirTicketSystem.modules.fare.Domain.Repositories;

namespace AirTicketSystem.modules.booking.Application.UseCases;

public sealed class CreateBookingUseCase
{
    private readonly IBookingRepository _bookingRepository;
    private readonly IClientRepository  _clientRepository;
    private readonly IFlightRepository  _flightRepository;
    private readonly IFareRepository    _fareRepository;

    public CreateBookingUseCase(
        IBookingRepository bookingRepository,
        IClientRepository  clientRepository,
        IFlightRepository  flightRepository,
        IFareRepository    fareRepository)
    {
        _bookingRepository = bookingRepository;
        _clientRepository  = clientRepository;
        _flightRepository  = flightRepository;
        _fareRepository    = fareRepository;
    }

    public async Task<Booking> ExecuteAsync(
        int clienteId,
        int vueloId,
        int tarifaId,
        decimal valorTotal,
        string? observaciones = null,
        CancellationToken cancellationToken = default)
    {
        var cliente = await _clientRepository.FindByIdAsync(clienteId)
            ?? throw new KeyNotFoundException(
                $"No se encontró un cliente con ID {clienteId}.");

        if (!cliente.EstaActivo)
            throw new InvalidOperationException(
                "El cliente no está activo y no puede realizar reservas.");

        var vuelo = await _flightRepository.FindByIdAsync(vueloId)
            ?? throw new KeyNotFoundException(
                $"No se encontró un vuelo con ID {vueloId}.");

        if (vuelo.Estado.Valor != "PROGRAMADO" && vuelo.Estado.Valor != "DEMORADO")
            throw new InvalidOperationException(
                $"El vuelo no está disponible para reservas. Estado actual: '{vuelo.Estado}'.");

        var tarifa = await _fareRepository.FindByIdAsync(tarifaId)
            ?? throw new KeyNotFoundException(
                $"No se encontró una tarifa con ID {tarifaId}.");

        if (!tarifa.Activa.Valor)
            throw new InvalidOperationException(
                "La tarifa seleccionada no está activa.");

        var booking = Booking.Crear(clienteId, vueloId, tarifaId, valorTotal, observaciones);
        await _bookingRepository.SaveAsync(booking);
        return booking;
    }
}
