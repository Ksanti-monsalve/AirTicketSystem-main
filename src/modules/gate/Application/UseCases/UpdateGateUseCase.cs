// src/modules/gate/Application/UseCases/UpdateGateUseCase.cs
using AirTicketSystem.modules.gate.Domain.Repositories;
using AirTicketSystem.modules.gate.Domain.aggregate;

namespace AirTicketSystem.modules.gate.Application.UseCases;

public class UpdateGateUseCase
{
    private readonly IGateRepository _repository;

    public UpdateGateUseCase(IGateRepository repository)
    {
        _repository = repository;
    }

    public async Task<Gate> ExecuteAsync(
        int id,
        string codigo,
        CancellationToken cancellationToken = default)
    {
        var puerta = await _repository.FindByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró una puerta de embarque con ID {id}.");

        if (!string.Equals(codigo.Trim(), puerta.Codigo.Valor, StringComparison.OrdinalIgnoreCase) &&
            await _repository.ExistsByCodigoAndTerminalAsync(
                codigo.Trim(), puerta.TerminalId))
            throw new InvalidOperationException(
                $"Ya existe otra puerta con el código '{codigo.Trim().ToUpperInvariant()}' " +
                "en la misma terminal.");

        puerta.ActualizarCodigo(codigo);
        await _repository.UpdateAsync(puerta);
        return puerta;
    }
}