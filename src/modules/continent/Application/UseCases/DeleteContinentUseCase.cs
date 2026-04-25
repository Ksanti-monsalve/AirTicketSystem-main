// src/modules/continent/Application/UseCases/DeleteContinentUseCase.cs
using AirTicketSystem.modules.continent.Domain.Repositories;

namespace AirTicketSystem.modules.continent.Application.UseCases;

public sealed class DeleteContinentUseCase
{
    private readonly IContinentRepository _repository;

    public DeleteContinentUseCase(IContinentRepository repository)
    {
        _repository = repository;
    }

    public async Task ExecuteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        _ = await _repository.FindByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró un continente con ID {id}.");

        await _repository.DeleteAsync(id);
    }
}
