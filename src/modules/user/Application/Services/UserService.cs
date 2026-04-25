// src/modules/user/Application/Services/UserService.cs
using AirTicketSystem.modules.user.Domain.aggregate;
using AirTicketSystem.modules.user.Application.Interfaces;
using AirTicketSystem.modules.user.Application.UseCases;

namespace AirTicketSystem.modules.user.Application.Services;

public sealed class UserService : IUserService
{
    private readonly CreateUserUseCase        _create;
    private readonly GetUserByIdUseCase       _getById;
    private readonly GetAllUsersUseCase       _getAll;
    private readonly GetUserByUsernameUseCase _getByUsername;
    private readonly LoginUseCase             _login;
    private readonly LogoutUseCase            _logout;
    private readonly ChangePasswordUseCase    _changePassword;
    private readonly ChangeRoleUseCase        _changeRole;
    private readonly ActivateUserUseCase      _activate;
    private readonly DeactivateUserUseCase    _deactivate;
    private readonly DeleteUserUseCase        _delete;

    public UserService(
        CreateUserUseCase create,
        GetUserByIdUseCase getById,
        GetAllUsersUseCase getAll,
        GetUserByUsernameUseCase getByUsername,
        LoginUseCase login,
        LogoutUseCase logout,
        ChangePasswordUseCase changePassword,
        ChangeRoleUseCase changeRole,
        ActivateUserUseCase activate,
        DeactivateUserUseCase deactivate,
        DeleteUserUseCase delete)
    {
        _create         = create;
        _getById        = getById;
        _getAll         = getAll;
        _getByUsername  = getByUsername;
        _login          = login;
        _logout         = logout;
        _changePassword = changePassword;
        _changeRole     = changeRole;
        _activate       = activate;
        _deactivate     = deactivate;
        _delete         = delete;
    }

    public Task<User> CreateAsync(
        int personaId, int rolId, string username, string passwordHash)
        => _create.ExecuteAsync(personaId, rolId, username, passwordHash);

    public Task<User> GetByIdAsync(int id)
        => _getById.ExecuteAsync(id);

    public Task<IReadOnlyCollection<User>> GetAllAsync()
        => _getAll.ExecuteAsync();

    public Task<User> GetByUsernameAsync(string username)
        => _getByUsername.ExecuteAsync(username);

    public Task<User> LoginAsync(string username, string passwordHash, string? ipAddress)
        => _login.ExecuteAsync(username, passwordHash, ipAddress);

    public Task LogoutAsync(int userId, string? ipAddress)
        => _logout.ExecuteAsync(userId, ipAddress);

    public Task<User> ChangePasswordAsync(int id, string nuevoPasswordHash)
        => _changePassword.ExecuteAsync(id, nuevoPasswordHash);

    public Task<User> ChangeRoleAsync(int id, int nuevoRolId)
        => _changeRole.ExecuteAsync(id, nuevoRolId);

    public Task<User> ActivateAsync(int id)
        => _activate.ExecuteAsync(id);

    public Task<User> DeactivateAsync(int id)
        => _deactivate.ExecuteAsync(id);

    public Task DeleteAsync(int id)
        => _delete.ExecuteAsync(id);
}
