// src/modules/luggage/Application/UseCases/GetLuggageByPassengerUseCase.cs
using AirTicketSystem.modules.luggage.Domain.aggregate;
using AirTicketSystem.modules.luggage.Domain.Repositories;

namespace AirTicketSystem.modules.luggage.Application.UseCases;

public sealed class GetLuggageByPassengerUseCase
{
    private readonly ILuggageRepository _repository;

    public GetLuggageByPassengerUseCase(ILuggageRepository repository) => _repository = repository;

    public async Task<IReadOnlyCollection<Luggage>> ExecuteAsync(
        int pasajeroReservaId, CancellationToken cancellationToken = default)
    {
        if (pasajeroReservaId <= 0)
            throw new ArgumentException("El ID del pasajero de reserva no es válido.");

        return await _repository.FindByPasajeroReservaAsync(pasajeroReservaId);
    }
}
