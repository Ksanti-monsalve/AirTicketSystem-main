// src/modules/user/Application/UseCases/GetAllUsersUseCase.cs
using AirTicketSystem.modules.user.Domain.aggregate;
using AirTicketSystem.modules.user.Domain.Repositories;

namespace AirTicketSystem.modules.user.Application.UseCases;

public sealed class GetAllUsersUseCase
{
    private readonly IUserRepository _repository;

    public GetAllUsersUseCase(IUserRepository repository) => _repository = repository;

    public Task<IReadOnlyCollection<User>> ExecuteAsync(
        CancellationToken cancellationToken = default)
        => _repository.FindAllAsync();
}
