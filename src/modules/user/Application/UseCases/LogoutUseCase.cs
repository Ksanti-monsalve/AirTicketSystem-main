// src/modules/user/Application/UseCases/LogoutUseCase.cs
using AirTicketSystem.modules.user.Domain.aggregate;
using AirTicketSystem.modules.user.Domain.Repositories;

namespace AirTicketSystem.modules.user.Application.UseCases;

public sealed class LogoutUseCase
{
    private readonly IUserRepository      _repository;
    private readonly IAccessLogRepository _logRepository;

    public LogoutUseCase(IUserRepository repository, IAccessLogRepository logRepository)
    {
        _repository    = repository;
        _logRepository = logRepository;
    }

    public async Task ExecuteAsync(
        int userId,
        string? ipAddress = null,
        CancellationToken cancellationToken = default)
    {
        if (userId <= 0)
            throw new ArgumentException("El ID del usuario no es válido.");

        _ = await _repository.FindByIdAsync(userId)
            ?? throw new KeyNotFoundException(
                $"No se encontró un usuario con ID {userId}.");

        await _logRepository.SaveAsync(AccessLog.CrearLogout(userId, ipAddress));
    }
}
