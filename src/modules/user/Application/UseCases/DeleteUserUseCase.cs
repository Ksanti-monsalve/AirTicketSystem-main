// src/modules/user/Application/UseCases/DeleteUserUseCase.cs
using AirTicketSystem.modules.user.Domain.Repositories;

namespace AirTicketSystem.modules.user.Application.UseCases;

public sealed class DeleteUserUseCase
{
    private readonly IUserRepository _repository;

    public DeleteUserUseCase(IUserRepository repository) => _repository = repository;

    public async Task ExecuteAsync(int id, CancellationToken cancellationToken = default)
    {
        if (id <= 0)
            throw new ArgumentException("El ID del usuario no es válido.");

        var user = await _repository.FindByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un usuario con ID {id}.");

        if (user.Activo.Valor)
            throw new InvalidOperationException(
                "No se puede eliminar un usuario activo. Desactívelo primero.");

        await _repository.DeleteAsync(id);
    }
}
