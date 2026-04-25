// src/modules/client/Application/UseCases/ActivateClientUseCase.cs
using AirTicketSystem.modules.client.Domain.aggregate;
using AirTicketSystem.modules.client.Domain.Repositories;

namespace AirTicketSystem.modules.client.Application.UseCases;

public sealed class ActivateClientUseCase
{
    private readonly IClientRepository _repository;

    public ActivateClientUseCase(IClientRepository repository) => _repository = repository;

    public async Task<Client> ExecuteAsync(int id, CancellationToken cancellationToken = default)
    {
        if (id <= 0)
            throw new ArgumentException("El ID del cliente no es válido.");

        var client = await _repository.FindByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un cliente con ID {id}.");

        client.Activar();
        await _repository.UpdateAsync(client);
        return client;
    }
}
