// src/modules/terminal/Application/UseCases/UpdateTerminalUseCase.cs
using AirTicketSystem.modules.terminal.Domain.Repositories;
using AirTicketSystem.modules.terminal.Domain.aggregate;
using AirTicketSystem.modules.terminal.Domain.ValueObjects;

namespace AirTicketSystem.modules.terminal.Application.UseCases;

public class UpdateTerminalUseCase
{
    private readonly ITerminalRepository _repository;

    public UpdateTerminalUseCase(ITerminalRepository repository)
    {
        _repository = repository;
    }

    public async Task<Terminal> ExecuteAsync(
        int id,
        string nombre,
        string? descripcion,
        CancellationToken cancellationToken = default)
    {
        var terminal = await _repository.FindByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró una terminal con ID {id}.");

        var nombreVO = NombreTerminal.Crear(nombre);

        if (nombreVO.Valor != terminal.Nombre.Valor &&
            await _repository.ExistsByNombreAndAeropuertoAsync(
                nombreVO.Valor, terminal.AeropuertoId))
            throw new InvalidOperationException(
                $"Ya existe otra terminal con el nombre '{nombreVO.Valor}' " +
                "en el mismo aeropuerto.");

        terminal.ActualizarNombre(nombreVO.Valor);
        terminal.ActualizarDescripcion(descripcion);

        await _repository.UpdateAsync(terminal);
        return terminal;
    }
}