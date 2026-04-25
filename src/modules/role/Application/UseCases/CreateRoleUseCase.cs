// src/modules/role/Application/UseCases/CreateRoleUseCase.cs
using AirTicketSystem.modules.role.Domain.aggregate;
using AirTicketSystem.modules.role.Domain.Repositories;

namespace AirTicketSystem.modules.role.Application.UseCases;

public sealed class CreateRoleUseCase
{
    private readonly IRoleRepository _repository;

    public CreateRoleUseCase(IRoleRepository repository) => _repository = repository;

    public async Task<Role> ExecuteAsync(string nombre, CancellationToken cancellationToken = default)
    {
        if (await _repository.ExistsByNombreAsync(nombre))
            throw new InvalidOperationException(
                $"Ya existe un rol con el nombre '{nombre}'.");

        var role = Role.Crear(nombre);
        await _repository.SaveAsync(role);
        return role;
    }
}
