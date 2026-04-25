// src/modules/user/Application/UseCases/LoginUseCase.cs
using AirTicketSystem.modules.user.Domain.aggregate;
using AirTicketSystem.modules.user.Domain.Repositories;

namespace AirTicketSystem.modules.user.Application.UseCases;

public sealed class LoginUseCase
{
    private readonly IUserRepository      _repository;
    private readonly IAccessLogRepository _logRepository;

    public LoginUseCase(IUserRepository repository, IAccessLogRepository logRepository)
    {
        _repository    = repository;
        _logRepository = logRepository;
    }

    public async Task<User> ExecuteAsync(
        string username,
        string passwordHash,
        string? ipAddress = null,
        CancellationToken cancellationToken = default)
    {
        var user = await _repository.FindByUsernameAsync(username)
            ?? throw new KeyNotFoundException(
                $"No se encontró un usuario con el username '{username}'.");

        if (user.EstaBloqueado)
            throw new InvalidOperationException(
                "La cuenta está bloqueada o inactiva.");

        if (user.PasswordHash.Valor != passwordHash)
        {
            user.RegistrarIntentoFallido();
            await _repository.UpdateAsync(user);
            await _logRepository.SaveAsync(
                AccessLog.CrearIntentoFallido(user.Id, ipAddress));
            throw new UnauthorizedAccessException("Credenciales incorrectas.");
        }

        user.RegistrarLoginExitoso();
        await _repository.UpdateAsync(user);
        await _logRepository.SaveAsync(AccessLog.CrearLogin(user.Id, ipAddress));
        return user;
    }
}
