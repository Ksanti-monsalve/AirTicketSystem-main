// src/modules/user/Application/UseCases/CreateUserUseCase.cs
using AirTicketSystem.modules.user.Domain.aggregate;
using AirTicketSystem.modules.user.Domain.Repositories;
using AirTicketSystem.modules.person.Domain.Repositories;
using AirTicketSystem.modules.role.Domain.Repositories;

namespace AirTicketSystem.modules.user.Application.UseCases;

public sealed class CreateUserUseCase
{
    private readonly IUserRepository   _repository;
    private readonly IPersonRepository _personRepository;
    private readonly IRoleRepository   _roleRepository;

    public CreateUserUseCase(
        IUserRepository repository,
        IPersonRepository personRepository,
        IRoleRepository roleRepository)
    {
        _repository        = repository;
        _personRepository  = personRepository;
        _roleRepository    = roleRepository;
    }

    public async Task<User> ExecuteAsync(
        int personaId,
        int rolId,
        string username,
        string passwordHash,
        CancellationToken cancellationToken = default)
    {
        _ = await _personRepository.FindByIdAsync(personaId)
            ?? throw new KeyNotFoundException(
                $"No se encontró una persona con ID {personaId}.");

        _ = await _roleRepository.FindByIdAsync(rolId)
            ?? throw new KeyNotFoundException(
                $"No se encontró un rol con ID {rolId}.");

        if (await _repository.ExistsByUsernameAsync(username))
            throw new InvalidOperationException(
                $"El nombre de usuario '{username}' ya está en uso.");

        var user = User.Crear(personaId, rolId, username, passwordHash);
        await _repository.SaveAsync(user);
        return user;
    }
}
