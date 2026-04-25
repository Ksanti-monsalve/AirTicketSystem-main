// src/modules/client/Application/UseCases/GetActiveClientsUseCase.cs
using AirTicketSystem.modules.client.Domain.aggregate;
using AirTicketSystem.modules.client.Domain.Repositories;

namespace AirTicketSystem.modules.client.Application.UseCases;

public sealed class GetActiveClientsUseCase
{
    private readonly IClientRepository _repository;

    public GetActiveClientsUseCase(IClientRepository repository) => _repository = repository;

    public Task<IReadOnlyCollection<Client>> ExecuteAsync(
        CancellationToken cancellationToken = default)
        => _repository.FindActivosAsync();
}
