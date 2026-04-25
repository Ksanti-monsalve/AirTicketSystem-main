// src/modules/ticket/Application/UseCases/VoidTicketUseCase.cs
using AirTicketSystem.modules.ticket.Domain.aggregate;
using AirTicketSystem.modules.ticket.Domain.Repositories;

namespace AirTicketSystem.modules.ticket.Application.UseCases;

public sealed class VoidTicketUseCase
{
    private readonly ITicketRepository _repository;

    public VoidTicketUseCase(ITicketRepository repository) => _repository = repository;

    public async Task<Ticket> ExecuteAsync(
        int id, CancellationToken cancellationToken = default)
    {
        if (id <= 0)
            throw new ArgumentException("El ID del tiquete no es válido.");

        var ticket = await _repository.FindByIdAsync(id)
            ?? throw new KeyNotFoundException($"No se encontró un tiquete con ID {id}.");

        ticket.Anular();
        await _repository.UpdateAsync(ticket);
        return ticket;
    }
}
