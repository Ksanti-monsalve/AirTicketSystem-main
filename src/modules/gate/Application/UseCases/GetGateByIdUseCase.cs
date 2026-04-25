// src/modules/gate/Application/UseCases/GetGateByIdUseCase.cs
using AirTicketSystem.modules.gate.Domain.Repositories;
using AirTicketSystem.modules.gate.Domain.aggregate;

namespace AirTicketSystem.modules.gate.Application.UseCases;

public class GetGateByIdUseCase
{
    private readonly IGateRepository _repository;

    public GetGateByIdUseCase(IGateRepository repository)
    {
        _repository = repository;
    }

    public async Task<Gate> ExecuteAsync(
        int id,
        CancellationToken cancellationToken = default)
    {
        if (id <= 0)
            throw new ArgumentException(
                "El ID de la puerta de embarque no es válido.");

        return await _repository.FindByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró una puerta de embarque con ID {id}.");
    }
}