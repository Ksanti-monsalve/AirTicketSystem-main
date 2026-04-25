// src/modules/airline/Application/Interfaces/IAirlineService.cs
using AirTicketSystem.modules.airline.Domain.aggregate;

namespace AirTicketSystem.modules.airline.Application.Interfaces;

public interface IAirlineService
{
    Task<Airline> CreateAsync(
        int paisId, string codigoIata, string codigoIcao,
        string nombre, string? nombreComercial, string? sitioWeb);
    Task<Airline> GetByIdAsync(int id);
    Task<Airline> GetByIataAsync(string codigoIata);
    Task<IReadOnlyCollection<Airline>> GetAllAsync();
    Task<IReadOnlyCollection<Airline>> GetActivasAsync();
    Task<Airline> UpdateAsync(int id, string nombre, string? nombreComercial, string? sitioWeb);
    Task ActivateAsync(int id);
    Task DeactivateAsync(int id);
    Task DeleteAsync(int id);
    Task<AirlinePhone> AddPhoneAsync(
        int aerolineaId, int tipoTelefonoId,
        string numero, string? indicativo, bool esPrincipal);
    Task RemovePhoneAsync(int phoneId);
    Task<AirlineEmail> AddEmailAsync(
        int aerolineaId, int tipoEmailId,
        string email, bool esPrincipal);
    Task RemoveEmailAsync(int emailId);
}
