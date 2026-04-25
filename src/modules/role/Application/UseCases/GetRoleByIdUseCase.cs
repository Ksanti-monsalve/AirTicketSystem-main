// src/modules/role/Application/UseCases/GetRoleByIdUseCase.cs
using AirTicketSystem.modules.role.Domain.aggregate;
using AirTicketSystem.modules.role.Domain.Repositories;

namespace AirTicketSystem.modules.role.Application.UseCases;

public sealed class GetRoleByIdUseCase
{
    private readonly IRoleRepository _repository;

    public GetRoleByIdUseCase(IRoleRepository repository) => _repository = repository;

    public async Task<Role> ExecuteAsync(int id, CancellationToken cancellationToken = default)
    {
        if (id <= 0)
            throw new ArgumentException("El ID del rol no es válido.");

        return await _repository.FindByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un rol con ID {id}.");
    }
}
