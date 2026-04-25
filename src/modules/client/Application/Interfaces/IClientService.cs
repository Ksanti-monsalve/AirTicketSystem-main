// src/modules/client/Application/Interfaces/IClientService.cs
using AirTicketSystem.modules.client.Domain.aggregate;

namespace AirTicketSystem.modules.client.Application.Interfaces;

public interface IClientService
{
    Task<Client> CreateAsync(int personaId, int usuarioId);
    Task<Client> GetByIdAsync(int id);
    Task<IReadOnlyCollection<Client>> GetAllAsync();
    Task<IReadOnlyCollection<Client>> GetActivosAsync();
    Task<Client> ActivateAsync(int id);
    Task<Client> DeactivateAsync(int id);
    Task DeleteAsync(int id);

    // Contactos de emergencia
    Task<EmergencyContact> AddEmergencyContactAsync(
        int clienteId, int personaId, int relacionId, bool esPrincipal);
    Task<EmergencyContact> SetPrincipalEmergencyContactAsync(int contactId);
    Task<EmergencyContact> UpdateEmergencyContactAsync(int contactId, int relacionId);
    Task DeleteEmergencyContactAsync(int contactId);
}
