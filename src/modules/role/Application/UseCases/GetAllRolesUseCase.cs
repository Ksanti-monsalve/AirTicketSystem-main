// src/modules/role/Application/UseCases/GetAllRolesUseCase.cs
using AirTicketSystem.modules.role.Domain.aggregate;
using AirTicketSystem.modules.role.Domain.Repositories;

namespace AirTicketSystem.modules.role.Application.UseCases;

public sealed class GetAllRolesUseCase
{
    private readonly IRoleRepository _repository;

    public GetAllRolesUseCase(IRoleRepository repository) => _repository = repository;

    public Task<IReadOnlyCollection<Role>> ExecuteAsync(CancellationToken cancellationToken = default)
        => _repository.FindAllAsync();
}
