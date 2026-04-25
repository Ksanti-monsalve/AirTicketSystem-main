// src/modules/terminal/Application/UseCases/DeleteTerminalUseCase.cs
using AirTicketSystem.modules.terminal.Domain.Repositories;

namespace AirTicketSystem.modules.terminal.Application.UseCases;

public class DeleteTerminalUseCase
{
    private readonly ITerminalRepository _repository;

    public DeleteTerminalUseCase(ITerminalRepository repository)
    {
        _repository = repository;
    }

    public async Task ExecuteAsync(int id)
    {
        var terminal = await _repository.FindByIdAsync(id)
            ?? throw new KeyNotFoundException(
                $"No se encontró una terminal con ID {id}.");

        // Si tiene puertas de embarque asociadas, la BD lanzará FK violation
        // que ConsoleErrorHandler convertirá en mensaje amigable
        await _repository.DeleteAsync(id);
    }
}