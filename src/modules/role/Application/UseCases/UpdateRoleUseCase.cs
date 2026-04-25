// src/modules/role/Application/UseCases/UpdateRoleUseCase.cs
using AirTicketSystem.modules.role.Domain.aggregate;
using AirTicketSystem.modules.role.Domain.Repositories;

namespace AirTicketSystem.modules.role.Application.UseCases;

public sealed class UpdateRoleUseCase
{
    private readonly IRoleRepository _repository;

    public UpdateRoleUseCase(IRoleRepository repository) => _repository = repository;

    public async Task<Role> ExecuteAsync(int id, string nombre, CancellationToken cancellationToken = default)
    {
        var role = await _repository.FindByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un rol con ID {id}.");

        if (await _repository.ExistsByNombreAsync(nombre) &&
            !role.Nombre.Valor.Equals(nombre, StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException(
                $"Ya existe un rol con el nombre '{nombre}'.");

        role.ActualizarNombre(nombre);
        await _repository.UpdateAsync(role);
        return role;
    }
}
