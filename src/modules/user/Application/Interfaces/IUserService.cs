// src/modules/user/Application/Interfaces/IUserService.cs
using AirTicketSystem.modules.user.Domain.aggregate;

namespace AirTicketSystem.modules.user.Application.Interfaces;

public interface IUserService
{
    Task<User> CreateAsync(int personaId, int rolId, string username, string passwordHash);
    Task<User> GetByIdAsync(int id);
    Task<IReadOnlyCollection<User>> GetAllAsync();
    Task<User> GetByUsernameAsync(string username);
    Task<User> LoginAsync(string username, string passwordHash, string? ipAddress);
    Task LogoutAsync(int userId, string? ipAddress);
    Task<User> ChangePasswordAsync(int id, string nuevoPasswordHash);
    Task<User> ChangeRoleAsync(int id, int nuevoRolId);
    Task<User> ActivateAsync(int id);
    Task<User> DeactivateAsync(int id);
    Task DeleteAsync(int id);
}
