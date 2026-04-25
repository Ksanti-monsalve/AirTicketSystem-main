// src/modules/user/Application/UseCases/GetUserByIdUseCase.cs
using AirTicketSystem.modules.user.Domain.aggregate;
using AirTicketSystem.modules.user.Domain.Repositories;

namespace AirTicketSystem.modules.user.Application.UseCases;

public sealed class GetUserByIdUseCase
{
    private readonly IUserRepository _repository;

    public GetUserByIdUseCase(IUserRepository repository) => _repository = repository;

    public async Task<User> ExecuteAsync(int id, CancellationToken cancellationToken = default)
    {
        if (id <= 0)
            throw new ArgumentException("El ID del usuario no es válido.");

        return await _repository.FindByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un usuario con ID {id}.");
    }
}
