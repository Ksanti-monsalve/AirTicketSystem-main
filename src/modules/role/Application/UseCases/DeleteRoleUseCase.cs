// src/modules/role/Application/UseCases/DeleteRoleUseCase.cs
using AirTicketSystem.modules.role.Domain.Repositories;

namespace AirTicketSystem.modules.role.Application.UseCases;

public sealed class DeleteRoleUseCase
{
    private readonly IRoleRepository _repository;

    public DeleteRoleUseCase(IRoleRepository repository) => _repository = repository;

    public async Task ExecuteAsync(int id, CancellationToken cancellationToken = default)
    {
        _ = await _repository.FindByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un rol con ID {id}.");

        await _repository.DeleteAsync(id);
    }
}
