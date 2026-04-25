// src/modules/user/Application/UseCases/GetUserByUsernameUseCase.cs
using AirTicketSystem.modules.user.Domain.aggregate;
using AirTicketSystem.modules.user.Domain.Repositories;

namespace AirTicketSystem.modules.user.Application.UseCases;

public sealed class GetUserByUsernameUseCase
{
    private readonly IUserRepository _repository;

    public GetUserByUsernameUseCase(IUserRepository repository) => _repository = repository;

    public async Task<User> ExecuteAsync(
        string username, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException("El nombre de usuario no puede estar vacío.");

        return await _repository.FindByUsernameAsync(username)
            ?? throw new KeyNotFoundException(
                $"No se encontró un usuario con el username '{username}'.");
    }
}
