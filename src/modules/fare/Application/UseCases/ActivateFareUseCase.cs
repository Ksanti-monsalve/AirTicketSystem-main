// src/modules/fare/Application/UseCases/ActivateFareUseCase.cs
using AirTicketSystem.modules.fare.Domain.Repositories;

namespace AirTicketSystem.modules.fare.Application.UseCases;

public sealed class ActivateFareUseCase
{
    private readonly IFareRepository _repository;

    public ActivateFareUseCase(IFareRepository repository) => _repository = repository;

    public async Task ExecuteAsync(int id, CancellationToken cancellationToken = default)
    {
        var fare = await _repository.FindByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró una tarifa con ID {id}.");

        fare.Activar();
        await _repository.UpdateAsync(fare);
    }
}
