// src/modules/checkin/Application/UseCases/GetCheckInByPassengerUseCase.cs
using AirTicketSystem.modules.checkin.Domain.aggregate;
using AirTicketSystem.modules.checkin.Domain.Repositories;

namespace AirTicketSystem.modules.checkin.Application.UseCases;

public sealed class GetCheckInByPassengerUseCase
{
    private readonly ICheckInRepository _repository;

    public GetCheckInByPassengerUseCase(ICheckInRepository repository)
        => _repository = repository;

    public async Task<CheckIn> ExecuteAsync(
        int pasajeroReservaId,
        CancellationToken cancellationToken = default)
    {
        if (pasajeroReservaId <= 0)
            throw new ArgumentException("El ID del pasajero-reserva no es válido.");

        return await _repository.FindByPasajeroReservaAsync(pasajeroReservaId)
            ?? throw new KeyNotFoundException(
                "No hay check-in para este pasajero. " +
                "Primero realice el check-in virtual.");
    }
}

