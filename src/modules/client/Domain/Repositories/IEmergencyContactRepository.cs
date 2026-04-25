// src/modules/client/Domain/Repositories/IEmergencyContactRepository.cs
using AirTicketSystem.modules.client.Domain.aggregate;

namespace AirTicketSystem.modules.client.Domain.Repositories;

public interface IEmergencyContactRepository
{
    Task<EmergencyContact?> FindByIdAsync(int id);
    Task<IReadOnlyCollection<EmergencyContact>> FindByClienteAsync(int clienteId);
    Task<EmergencyContact?> FindPrincipalByClienteAsync(int clienteId);
    Task<bool> ExistsByClienteAndPersonaAsync(int clienteId, int personaId);
    Task SaveAsync(EmergencyContact contact);
    Task UpdateAsync(EmergencyContact contact);
    Task DeleteAsync(int id);
    Task DesmarcarPrincipalByClienteAsync(int clienteId);
}
