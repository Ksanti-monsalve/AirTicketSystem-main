// src/modules/terminal/Application/Interfaces/ITerminalService.cs
using AirTicketSystem.modules.terminal.Domain.aggregate;

namespace AirTicketSystem.modules.terminal.Application.Interfaces;

public interface ITerminalService
{
    Task<Terminal> CreateAsync(
        int aeropuertoId, string nombre, string? descripcion);
    Task<Terminal> GetByIdAsync(int id);
    Task<IReadOnlyCollection<Terminal>> GetByAirportAsync(int aeropuertoId);
    Task<Terminal> UpdateAsync(
        int id, string nombre, string? descripcion);
    Task DeleteAsync(int id);
}