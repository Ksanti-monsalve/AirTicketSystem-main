// src/modules/client/Domain/Repositories/IClientRepository.cs
using AirTicketSystem.modules.client.Domain.aggregate;

namespace AirTicketSystem.modules.client.Domain.Repositories;

public interface IClientRepository
{
    Task<Client?> FindByIdAsync(int id);
    Task<IReadOnlyCollection<Client>> FindAllAsync();
    Task<Client?> FindByPersonaAsync(int personaId);
    Task<Client?> FindByUsuarioAsync(int usuarioId);
    Task<IReadOnlyCollection<Client>> FindActivosAsync();
    Task SaveAsync(Client client);
    Task UpdateAsync(Client client);
    Task DeleteAsync(int id);
}
