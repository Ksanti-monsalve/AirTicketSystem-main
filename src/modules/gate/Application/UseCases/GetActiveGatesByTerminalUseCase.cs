// src/modules/gate/Application/UseCases/GetActiveGatesByTerminalUseCase.cs
using AirTicketSystem.modules.gate.Domain.Repositories;
using AirTicketSystem.modules.gate.Domain.aggregate;

namespace AirTicketSystem.modules.gate.Application.UseCases;

public class GetActiveGatesByTerminalUseCase
{
    private readonly IGateRepository _repository;

    public GetActiveGatesByTerminalUseCase(IGateRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyCollection<Gate>> ExecuteAsync(
        int terminalId,
        CancellationToken cancellationToken = default)
    {
        if (terminalId <= 0)
            throw new ArgumentException("El ID de la terminal no es válido.");

        return await _repository.FindActivasByTerminalAsync(terminalId);
    }
}