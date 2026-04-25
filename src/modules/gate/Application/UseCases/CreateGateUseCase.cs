// src/modules/gate/Application/UseCases/CreateGateUseCase.cs
using AirTicketSystem.modules.gate.Domain.Repositories;
using AirTicketSystem.modules.gate.Domain.aggregate;
using AirTicketSystem.modules.gate.Domain.ValueObjects;
using AirTicketSystem.modules.terminal.Domain.Repositories;

namespace AirTicketSystem.modules.gate.Application.UseCases;

public class CreateGateUseCase
{
    private readonly IGateRepository _repository;
    private readonly ITerminalRepository _terminalRepository;

    public CreateGateUseCase(
        IGateRepository repository,
        ITerminalRepository terminalRepository)
    {
        _repository         = repository;
        _terminalRepository = terminalRepository;
    }

    public async Task<Gate> ExecuteAsync(
        int terminalId,
        string codigo,
        CancellationToken cancellationToken = default)
    {
        _ = await _terminalRepository.FindByIdAsync(terminalId)
            ?? throw new KeyNotFoundException(
                $"No se encontró una terminal con ID {terminalId}.");

        var codigoVO = CodigoGate.Crear(codigo);

        if (await _repository.ExistsByCodigoAndTerminalAsync(
            codigoVO.Valor, terminalId))
            throw new InvalidOperationException(
                $"Ya existe una puerta con el código '{codigoVO.Valor}' " +
                $"en la terminal con ID {terminalId}.");

        var gate = Gate.Crear(terminalId, codigoVO.Valor);
        await _repository.SaveAsync(gate);
        return gate;
    }
}