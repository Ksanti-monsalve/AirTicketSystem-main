// src/modules/terminal/Domain/Repositories/ITerminalRepository.cs
using AirTicketSystem.modules.terminal.Domain.aggregate;

namespace AirTicketSystem.modules.terminal.Domain.Repositories;

public interface ITerminalRepository
{
    Task<Terminal?> FindByIdAsync(int id);
    Task<IReadOnlyCollection<Terminal>> FindByAeropuertoAsync(int aeropuertoId);
    Task<bool> ExistsByNombreAndAeropuertoAsync(string nombre, int aeropuertoId);
    Task SaveAsync(Terminal terminal);
    Task UpdateAsync(Terminal terminal);
    Task DeleteAsync(int id);
}