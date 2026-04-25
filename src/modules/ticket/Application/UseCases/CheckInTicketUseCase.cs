// src/modules/ticket/Application/UseCases/CheckInTicketUseCase.cs
using AirTicketSystem.modules.ticket.Domain.aggregate;
using AirTicketSystem.modules.ticket.Domain.Repositories;

namespace AirTicketSystem.modules.ticket.Application.UseCases;

public sealed class CheckInTicketUseCase
{
    private readonly ITicketRepository _repository;

    public CheckInTicketUseCase(ITicketRepository repository) => _repository = repository;

    public async Task<Ticket> ExecuteAsync(
        int id,
        int? asientoConfirmadoId = null,
        CancellationToken cancellationToken = default)
    {
        if (id <= 0)
            throw new ArgumentException("El ID del tiquete no es válido.");

        var ticket = await _repository.FindByIdAsync(id)
            ?? throw new KeyNotFoundException($"No se encontró un tiquete con ID {id}.");

        ticket.RegistrarCheckin(asientoConfirmadoId);
        await _repository.UpdateAsync(ticket);
        return ticket;
    }
}
