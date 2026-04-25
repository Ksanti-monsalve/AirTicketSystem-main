// src/modules/fare/Application/UseCases/DeactivateFareUseCase.cs
using AirTicketSystem.modules.fare.Domain.Repositories;

namespace AirTicketSystem.modules.fare.Application.UseCases;

public sealed class DeactivateFareUseCase
{
    private readonly IFareRepository _repository;

    public DeactivateFareUseCase(IFareRepository repository) => _repository = repository;

    public async Task ExecuteAsync(int id, CancellationToken cancellationToken = default)
    {
        var fare = await _repository.FindByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró una tarifa con ID {id}.");

        fare.Desactivar();
        await _repository.UpdateAsync(fare);
    }
}
