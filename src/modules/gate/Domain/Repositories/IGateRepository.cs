// src/modules/gate/Domain/Repositories/IGateRepository.cs
using AirTicketSystem.modules.gate.Domain.aggregate;

namespace AirTicketSystem.modules.gate.Domain.Repositories;

public interface IGateRepository
{
    Task<Gate?> FindByIdAsync(int id);
    Task<IReadOnlyCollection<Gate>> FindByTerminalAsync(int terminalId);
    Task<IReadOnlyCollection<Gate>> FindActivasByTerminalAsync(int terminalId);
    Task<bool> ExistsByCodigoAndTerminalAsync(string codigo, int terminalId);
    Task SaveAsync(Gate gate);
    Task UpdateAsync(Gate gate);
    Task DeleteAsync(int id);
}