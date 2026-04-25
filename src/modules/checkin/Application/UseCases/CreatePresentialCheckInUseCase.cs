// src/modules/checkin/Application/UseCases/CreatePresentialCheckInUseCase.cs
using AirTicketSystem.modules.checkin.Domain.aggregate;
using AirTicketSystem.modules.checkin.Domain.Repositories;
using AirTicketSystem.modules.bookingpassenger.Domain.Repositories;
using AirTicketSystem.modules.worker.Domain.Repositories;

namespace AirTicketSystem.modules.checkin.Application.UseCases;

public sealed class CreatePresentialCheckInUseCase
{
    private readonly ICheckInRepository          _checkInRepository;
    private readonly IBookingPassengerRepository _passengerRepository;
    private readonly IWorkerRepository           _workerRepository;

    public CreatePresentialCheckInUseCase(
        ICheckInRepository          checkInRepository,
        IBookingPassengerRepository passengerRepository,
        IWorkerRepository           workerRepository)
    {
        _checkInRepository   = checkInRepository;
        _passengerRepository = passengerRepository;
        _workerRepository    = workerRepository;
    }

    public async Task<CheckIn> ExecuteAsync(
        int pasajeroReservaId,
        int trabajadorId,
        CancellationToken cancellationToken = default)
    {
        _ = await _passengerRepository.FindByIdAsync(pasajeroReservaId)
            ?? throw new KeyNotFoundException(
                $"No se encontró un pasajero de reserva con ID {pasajeroReservaId}.");

        var worker = await _workerRepository.FindByIdAsync(trabajadorId)
            ?? throw new KeyNotFoundException(
                $"No se encontró un trabajador con ID {trabajadorId}.");

        if (!worker.EstaActivo)
            throw new InvalidOperationException(
                "El trabajador asignado no está activo.");

        var existing = await _checkInRepository.FindByPasajeroReservaAsync(pasajeroReservaId);
        if (existing is not null)
            throw new InvalidOperationException(
                "Ya existe un check-in registrado para este pasajero en esta reserva.");

        var checkIn = CheckIn.CrearPresencial(pasajeroReservaId, trabajadorId);
        await _checkInRepository.SaveAsync(checkIn);
        return checkIn;
    }
}
