// src/modules/gate/Application/Services/GateService.cs
using AirTicketSystem.modules.gate.Application.Interfaces;
using AirTicketSystem.modules.gate.Application.UseCases;
using AirTicketSystem.modules.gate.Domain.aggregate;

namespace AirTicketSystem.modules.gate.Application.Services;

public class GateService : IGateService
{
    private readonly CreateGateUseCase _create;
    private readonly GetGateByIdUseCase _getById;
    private readonly GetGatesByTerminalUseCase _getByTerminal;
    private readonly GetActiveGatesByTerminalUseCase _getActivas;
    private readonly UpdateGateUseCase _update;
    private readonly ActivateGateUseCase _activate;
    private readonly DeactivateGateUseCase _deactivate;
    private readonly DeleteGateUseCase _delete;

    public GateService(
        CreateGateUseCase create,
        GetGateByIdUseCase getById,
        GetGatesByTerminalUseCase getByTerminal,
        GetActiveGatesByTerminalUseCase getActivas,
        UpdateGateUseCase update,
        ActivateGateUseCase activate,
        DeactivateGateUseCase deactivate,
        DeleteGateUseCase delete)
    {
        _create       = create;
        _getById      = getById;
        _getByTerminal = getByTerminal;
        _getActivas   = getActivas;
        _update       = update;
        _activate     = activate;
        _deactivate   = deactivate;
        _delete       = delete;
    }

    public Task<Gate> CreateAsync(int terminalId, string codigo)
        => _create.ExecuteAsync(terminalId, codigo);

    public Task<Gate> GetByIdAsync(int id)
        => _getById.ExecuteAsync(id);

    public Task<IReadOnlyCollection<Gate>> GetByTerminalAsync(int terminalId)
        => _getByTerminal.ExecuteAsync(terminalId);

    public Task<IReadOnlyCollection<Gate>> GetActivasByTerminalAsync(int terminalId)
        => _getActivas.ExecuteAsync(terminalId);

    public Task<Gate> UpdateAsync(int id, string codigo)
        => _update.ExecuteAsync(id, codigo);

    public Task ActivateAsync(int id)
        => _activate.ExecuteAsync(id);

    public Task DeactivateAsync(int id)
        => _deactivate.ExecuteAsync(id);

    public Task DeleteAsync(int id)
        => _delete.ExecuteAsync(id);
}