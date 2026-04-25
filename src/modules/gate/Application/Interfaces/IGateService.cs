// src/modules/gate/Application/Interfaces/IGateService.cs
using AirTicketSystem.modules.gate.Domain.aggregate;

namespace AirTicketSystem.modules.gate.Application.Interfaces;

public interface IGateService
{
    Task<Gate> CreateAsync(int terminalId, string codigo);
    Task<Gate> GetByIdAsync(int id);
    Task<IReadOnlyCollection<Gate>> GetByTerminalAsync(int terminalId);
    Task<IReadOnlyCollection<Gate>> GetActivasByTerminalAsync(int terminalId);
    Task<Gate> UpdateAsync(int id, string codigo);
    Task ActivateAsync(int id);
    Task DeactivateAsync(int id);
    Task DeleteAsync(int id);
}