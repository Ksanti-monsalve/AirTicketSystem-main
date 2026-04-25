// src/modules/user/Application/UseCases/ChangeRoleUseCase.cs
using AirTicketSystem.modules.user.Domain.aggregate;
using AirTicketSystem.modules.user.Domain.Repositories;
using AirTicketSystem.modules.role.Domain.Repositories;

namespace AirTicketSystem.modules.user.Application.UseCases;

public sealed class ChangeRoleUseCase
{
    private readonly IUserRepository _repository;
    private readonly IRoleRepository _roleRepository;

    public ChangeRoleUseCase(IUserRepository repository, IRoleRepository roleRepository)
    {
        _repository     = repository;
        _roleRepository = roleRepository;
    }

    public async Task<User> ExecuteAsync(
        int id,
        int nuevoRolId,
        CancellationToken cancellationToken = default)
    {
        if (id <= 0)
            throw new ArgumentException("El ID del usuario no es válido.");

        var user = await _repository.FindByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un usuario con ID {id}.");

        _ = await _roleRepository.FindByIdAsync(nuevoRolId)
            ?? throw new KeyNotFoundException(
                $"No se encontró un rol con ID {nuevoRolId}.");

        user.CambiarRol(nuevoRolId);
        await _repository.UpdateAsync(user);
        return user;
    }
}
