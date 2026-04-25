// src/modules/role/Application/Services/RoleService.cs
using AirTicketSystem.modules.role.Application.Interfaces;
using AirTicketSystem.modules.role.Application.UseCases;
using AirTicketSystem.modules.role.Domain.aggregate;

namespace AirTicketSystem.modules.role.Application.Services;

public sealed class RoleService : IRoleService
{
    private readonly CreateRoleUseCase _create;
    private readonly GetRoleByIdUseCase _getById;
    private readonly GetAllRolesUseCase _getAll;
    private readonly UpdateRoleUseCase _update;
    private readonly DeleteRoleUseCase _delete;

    public RoleService(
        CreateRoleUseCase create,
        GetRoleByIdUseCase getById,
        GetAllRolesUseCase getAll,
        UpdateRoleUseCase update,
        DeleteRoleUseCase delete)
    {
        _create  = create;
        _getById = getById;
        _getAll  = getAll;
        _update  = update;
        _delete  = delete;
    }

    public Task<Role> CreateAsync(string nombre)
        => _create.ExecuteAsync(nombre);

    public Task<Role> GetByIdAsync(int id)
        => _getById.ExecuteAsync(id);

    public Task<IReadOnlyCollection<Role>> GetAllAsync()
        => _getAll.ExecuteAsync();

    public Task<Role> UpdateAsync(int id, string nombre)
        => _update.ExecuteAsync(id, nombre);

    public Task DeleteAsync(int id) => _delete.ExecuteAsync(id);
}
