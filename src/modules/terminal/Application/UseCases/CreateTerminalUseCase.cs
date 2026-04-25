// src/modules/terminal/Application/UseCases/CreateTerminalUseCase.cs
using AirTicketSystem.modules.terminal.Domain.Repositories;
using AirTicketSystem.modules.terminal.Domain.aggregate;
using AirTicketSystem.modules.terminal.Domain.ValueObjects;
using AirTicketSystem.modules.airport.Domain.Repositories;

namespace AirTicketSystem.modules.terminal.Application.UseCases;

public sealed class CreateTerminalUseCase
{
    private readonly ITerminalRepository _repository;
    private readonly IAirportRepository  _airportRepository;

    public CreateTerminalUseCase(
        ITerminalRepository repository,
        IAirportRepository  airportRepository)
    {
        _repository        = repository;
        _airportRepository = airportRepository;
    }

    public async Task<Terminal> ExecuteAsync(
        int aeropuertoId,
        string nombre,
        string? descripcion,
        CancellationToken cancellationToken = default)
    {
        var aeropuerto = await _airportRepository.FindByIdAsync(aeropuertoId)
            ?? throw new KeyNotFoundException(
                $"No se encontró un aeropuerto con ID {aeropuertoId}.");

        if (!aeropuerto.Activo.Valor)
            throw new InvalidOperationException(
                $"No se puede agregar una terminal al aeropuerto " +
                $"'{aeropuerto.Nombre.Valor}' porque está inactivo.");

        var nombreVO = NombreTerminal.Crear(nombre);

        if (await _repository.ExistsByNombreAndAeropuertoAsync(
            nombreVO.Valor, aeropuertoId))
            throw new InvalidOperationException(
                $"Ya existe una terminal con el nombre '{nombreVO.Valor}' " +
                $"en el aeropuerto '{aeropuerto.Nombre.Valor}'.");

        var terminal = Terminal.Crear(aeropuertoId, nombreVO.Valor, descripcion);
        await _repository.SaveAsync(terminal);
        return terminal;
    }
}
