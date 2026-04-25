// src/modules/fare/Application/UseCases/DeleteFareUseCase.cs
using AirTicketSystem.modules.fare.Domain.Repositories;

namespace AirTicketSystem.modules.fare.Application.UseCases;

public sealed class DeleteFareUseCase
{
    private readonly IFareRepository _repository;

    public DeleteFareUseCase(IFareRepository repository) => _repository = repository;

    public async Task ExecuteAsync(int id, CancellationToken cancellationToken = default)
    {
        var fare = await _repository.FindByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró una tarifa con ID {id}.");

        if (fare.Activa.Valor)
            throw new InvalidOperationException(
                "No se puede eliminar una tarifa activa. Desactívela primero.");

        await _repository.DeleteAsync(id);
    }
}
