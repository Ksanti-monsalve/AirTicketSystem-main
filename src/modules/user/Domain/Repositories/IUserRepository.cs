// src/modules/user/Domain/Repositories/IUserRepository.cs
using AirTicketSystem.modules.user.Domain.aggregate;

namespace AirTicketSystem.modules.user.Domain.Repositories;

public interface IUserRepository
{
    Task<User?> FindByIdAsync(int id);
    Task<IReadOnlyCollection<User>> FindAllAsync();
    Task<User?> FindByUsernameAsync(string username);
    Task<User?> FindByPersonaAsync(int personaId);
    Task<IReadOnlyCollection<User>> FindByRolAsync(int rolId);
    Task<IReadOnlyCollection<User>> FindActivosAsync();
    Task<bool> ExistsByUsernameAsync(string username);
    Task SaveAsync(User user);
    Task UpdateAsync(User user);
    Task DeleteAsync(int id);
}
