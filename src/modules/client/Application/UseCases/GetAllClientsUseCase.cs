// src/modules/client/Application/UseCases/GetAllClientsUseCase.cs
using AirTicketSystem.modules.client.Domain.aggregate;
using AirTicketSystem.modules.client.Domain.Repositories;

namespace AirTicketSystem.modules.client.Application.UseCases;

public sealed class GetAllClientsUseCase
{
    private readonly IClientRepository _repository;

    public GetAllClientsUseCase(IClientRepository repository) => _repository = repository;

    public Task<IReadOnlyCollection<Client>> ExecuteAsync(
        CancellationToken cancellationToken = default)
        => _repository.FindAllAsync();
}
