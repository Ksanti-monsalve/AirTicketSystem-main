// src/modules/gate/Application/UseCases/ActivateGateUseCase.cs
using AirTicketSystem.modules.gate.Domain.Repositories;

namespace AirTicketSystem.modules.gate.Application.UseCases;

public class ActivateGateUseCase
{
    private readonly IGateRepository _repository;

    public ActivateGateUseCase(IGateRepository repository)
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
                $"La puerta '{puerta.Codigo.Valor}' ya se encuentra activa.");

        puerta.Activar();
        await _repository.UpdateAsync(puerta);
    }
}