// src/modules/airport/Application/Interfaces/IAirportService.cs
using AirTicketSystem.modules.airport.Domain.aggregate;

namespace AirTicketSystem.modules.airport.Application.Interfaces;

public interface IAirportService
{
    Task<Airport> CreateAsync(
        int ciudadId, string codigoIata, string codigoIcao,
        string nombre, string? direccion);
    Task<Airport> GetByIdAsync(int id);
    Task<Airport> GetByIataAsync(string codigoIata);
    Task<IReadOnlyCollection<Airport>> GetAllAsync();
    Task<IReadOnlyCollection<Airport>> GetActivosAsync();
    Task<IReadOnlyCollection<Airport>> GetByCityAsync(int ciudadId);
    Task<Airport> UpdateAsync(
        int id, string nombre, string? direccion);
    Task ActivateAsync(int id);
    Task DeactivateAsync(int id);
    Task DeleteAsync(int id);
}