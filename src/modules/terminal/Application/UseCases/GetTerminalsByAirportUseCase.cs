// src/modules/terminal/Application/UseCases/GetTerminalsByAirportUseCase.cs
using AirTicketSystem.modules.terminal.Domain.Repositories;
using AirTicketSystem.modules.terminal.Domain.aggregate;

namespace AirTicketSystem.modules.terminal.Application.UseCases;

public class GetTerminalsByAirportUseCase
{
    private readonly ITerminalRepository _repository;

    public GetTerminalsByAirportUseCase(ITerminalRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyCollection<Terminal>> ExecuteAsync(
        int aeropuertoId,
        CancellationToken cancellationToken = default)
    {
        if (aeropuertoId <= 0)
            throw new ArgumentException("El ID del aeropuerto no es válido.");

        return await _repository.FindByAeropuertoAsync(aeropuertoId);
    }
}