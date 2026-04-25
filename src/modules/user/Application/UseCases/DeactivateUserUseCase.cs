// src/modules/user/Application/UseCases/DeactivateUserUseCase.cs
using AirTicketSystem.modules.user.Domain.aggregate;
using AirTicketSystem.modules.user.Domain.Repositories;

namespace AirTicketSystem.modules.user.Application.UseCases;

public sealed class DeactivateUserUseCase
{
    private readonly IUserRepository _repository;

    public DeactivateUserUseCase(IUserRepository repository) => _repository = repository;

    public async Task<User> ExecuteAsync(int id, CancellationToken cancellationToken = default)
    {
        if (id <= 0)
            throw new ArgumentException("El ID del usuario no es válido.");

        var user = await _repository.FindByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un usuario con ID {id}.");

        user.Desactivar();
        await _repository.UpdateAsync(user);
        return user;
    }
}
