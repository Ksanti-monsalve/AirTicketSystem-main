// src/modules/client/Application/UseCases/DeleteClientUseCase.cs
using AirTicketSystem.modules.client.Domain.Repositories;

namespace AirTicketSystem.modules.client.Application.UseCases;

public sealed class DeleteClientUseCase
{
    private readonly IClientRepository _repository;

    public DeleteClientUseCase(IClientRepository repository) => _repository = repository;

    public async Task ExecuteAsync(int id, CancellationToken cancellationToken = default)
    {
        if (id <= 0)
            throw new ArgumentException("El ID del cliente no es válido.");

        var client = await _repository.FindByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un cliente con ID {id}.");

        if (client.EstaActivo)
            throw new InvalidOperationException(
                "No se puede eliminar un cliente activo. Desactívelo primero.");

        await _repository.DeleteAsync(id);
    }
}
