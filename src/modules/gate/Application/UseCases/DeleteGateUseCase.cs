// src/modules/gate/Application/UseCases/DeleteGateUseCase.cs
using AirTicketSystem.modules.gate.Domain.Repositories;

namespace AirTicketSystem.modules.gate.Application.UseCases;

public class DeleteGateUseCase
{
    private readonly IGateRepository _repository;

    public DeleteGateUseCase(IGateRepository repository)
    {
        _repository = repository;
    }

    public async Task ExecuteAsync(int id)
    {
        var puerta = await _repository.FindByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró una puerta de embarque con ID {id}.");

        if (puerta.Activa.Valor)
            throw new InvalidOperationException(
                $"No se puede eliminar la puerta '{puerta.Codigo.Valor}' " +
                "porque está activa. Desactívela primero.");

        await _repository.DeleteAsync(id);
    }
}