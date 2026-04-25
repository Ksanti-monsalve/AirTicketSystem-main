// src/modules/user/Application/UseCases/ChangePasswordUseCase.cs
using AirTicketSystem.modules.user.Domain.aggregate;
using AirTicketSystem.modules.user.Domain.Repositories;

namespace AirTicketSystem.modules.user.Application.UseCases;

public sealed class ChangePasswordUseCase
{
    private readonly IUserRepository _repository;

    public ChangePasswordUseCase(IUserRepository repository) => _repository = repository;

    public async Task<User> ExecuteAsync(
        int id,
        string nuevoPasswordHash,
        CancellationToken cancellationToken = default)
    {
        if (id <= 0)
            throw new ArgumentException("El ID del usuario no es válido.");

        var user = await _repository.FindByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un usuario con ID {id}.");

        user.CambiarPassword(nuevoPasswordHash);
        await _repository.UpdateAsync(user);
        return user;
    }
}
