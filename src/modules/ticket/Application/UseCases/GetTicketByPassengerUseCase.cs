// src/modules/ticket/Application/UseCases/GetTicketByPassengerUseCase.cs
using AirTicketSystem.modules.ticket.Domain.aggregate;
using AirTicketSystem.modules.ticket.Domain.Repositories;

namespace AirTicketSystem.modules.ticket.Application.UseCases;

public sealed class GetTicketByPassengerUseCase
{
    private readonly ITicketRepository _repository;

    public GetTicketByPassengerUseCase(ITicketRepository repository)
        => _repository = repository;

    public async Task<Ticket> ExecuteAsync(
        int pasajeroReservaId,
        CancellationToken cancellationToken = default)
    {
        if (pasajeroReservaId <= 0)
            throw new ArgumentException("El ID del pasajero-reserva no es válido.");

        return await _repository.FindByPasajeroReservaAsync(pasajeroReservaId)
            ?? throw new KeyNotFoundException(
                "No se encontró un tiquete para este pasajero. " +
                "Primero emita el tiquete desde 'Emitir tiquete'.");
    }
}

