// src/modules/ticket/Application/UseCases/GetTicketByCodeUseCase.cs
using AirTicketSystem.modules.ticket.Domain.aggregate;
using AirTicketSystem.modules.ticket.Domain.Repositories;

namespace AirTicketSystem.modules.ticket.Application.UseCases;

public sealed class GetTicketByCodeUseCase
{
    private readonly ITicketRepository _repository;

    public GetTicketByCodeUseCase(ITicketRepository repository) => _repository = repository;

    public async Task<Ticket> ExecuteAsync(
        string codigoTiquete, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(codigoTiquete))
            throw new ArgumentException("El código del tiquete no puede estar vacío.");

        return await _repository.FindByCodigoTiqueteAsync(codigoTiquete)
            ?? throw new KeyNotFoundException(
                $"No se encontró un tiquete con código '{codigoTiquete}'.");
    }
}
