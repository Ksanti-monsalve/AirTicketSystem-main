// src/modules/checkin/Application/UseCases/CreateVirtualCheckInUseCase.cs
using AirTicketSystem.modules.checkin.Domain.aggregate;
using AirTicketSystem.modules.checkin.Domain.Repositories;
using AirTicketSystem.modules.bookingpassenger.Domain.Repositories;

namespace AirTicketSystem.modules.checkin.Application.UseCases;

public sealed class CreateVirtualCheckInUseCase
{
    private readonly ICheckInRepository          _checkInRepository;
    private readonly IBookingPassengerRepository _passengerRepository;

    public CreateVirtualCheckInUseCase(
        ICheckInRepository          checkInRepository,
        IBookingPassengerRepository passengerRepository)
    {
        _checkInRepository   = checkInRepository;
        _passengerRepository = passengerRepository;
    }

    public async Task<CheckIn> ExecuteAsync(
        int pasajeroReservaId, CancellationToken cancellationToken = default)
    {
        _ = await _passengerRepository.FindByIdAsync(pasajeroReservaId)
            ?? throw new KeyNotFoundException(
                $"No se encontró un pasajero de reserva con ID {pasajeroReservaId}.");

        var existing = await _checkInRepository.FindByPasajeroReservaAsync(pasajeroReservaId);
        if (existing is not null)
            throw new InvalidOperationException(
                "Ya existe un check-in registrado para este pasajero en esta reserva.");

        var checkIn = CheckIn.CrearVirtual(pasajeroReservaId);
        await _checkInRepository.SaveAsync(checkIn);
        return checkIn;
    }
}
