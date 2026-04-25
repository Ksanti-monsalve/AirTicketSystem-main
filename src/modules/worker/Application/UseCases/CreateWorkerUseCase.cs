// src/modules/worker/Application/UseCases/CreateWorkerUseCase.cs
using AirTicketSystem.modules.worker.Domain.aggregate;
using AirTicketSystem.modules.worker.Domain.Repositories;
using AirTicketSystem.modules.person.Domain.Repositories;
using AirTicketSystem.modules.workertype.Domain.Repositories;
using AirTicketSystem.modules.airport.Domain.Repositories;
using AirTicketSystem.modules.airline.Domain.Repositories;

namespace AirTicketSystem.modules.worker.Application.UseCases;

public sealed class CreateWorkerUseCase
{
    private readonly IWorkerRepository    _repository;
    private readonly IPersonRepository    _personRepository;
    private readonly IWorkerTypeRepository _workerTypeRepository;
    private readonly IAirportRepository   _airportRepository;
    private readonly IAirlineRepository   _airlineRepository;

    public CreateWorkerUseCase(
        IWorkerRepository repository,
        IPersonRepository personRepository,
        IWorkerTypeRepository workerTypeRepository,
        IAirportRepository airportRepository,
        IAirlineRepository airlineRepository)
    {
        _repository           = repository;
        _personRepository     = personRepository;
        _workerTypeRepository = workerTypeRepository;
        _airportRepository    = airportRepository;
        _airlineRepository    = airlineRepository;
    }

    public async Task<Worker> ExecuteAsync(
        int personaId,
        int tipoTrabajadorId,
        int aeropuertoBaseId,
        DateOnly fechaContratacion,
        decimal salario,
        int? aerolineaId = null,
        int? usuarioId = null,
        CancellationToken cancellationToken = default)
    {
        _ = await _personRepository.FindByIdAsync(personaId)
            ?? throw new KeyNotFoundException(
                $"No se encontró una persona con ID {personaId}.");

        if (await _repository.FindByPersonaAsync(personaId) is not null)
            throw new InvalidOperationException(
                "Esta persona ya tiene un registro como trabajador.");

        _ = await _workerTypeRepository.FindByIdAsync(tipoTrabajadorId)
            ?? throw new KeyNotFoundException(
                $"No se encontró un tipo de trabajador con ID {tipoTrabajadorId}.");

        var aeropuerto = await _airportRepository.FindByIdAsync(aeropuertoBaseId)
            ?? throw new KeyNotFoundException(
                $"No se encontró un aeropuerto con ID {aeropuertoBaseId}.");

        if (!aeropuerto.Activo.Valor)
            throw new InvalidOperationException(
                $"El aeropuerto '{aeropuerto.Nombre.Valor}' está inactivo.");

        if (aerolineaId.HasValue)
        {
            var aerolinea = await _airlineRepository.FindByIdAsync(aerolineaId.Value)
                ?? throw new KeyNotFoundException(
                    $"No se encontró una aerolínea con ID {aerolineaId.Value}.");

            if (!aerolinea.Activa.Valor)
                throw new InvalidOperationException(
                    $"La aerolínea '{aerolinea.Nombre.Valor}' está inactiva.");
        }

        var worker = Worker.Crear(
            personaId, tipoTrabajadorId, aeropuertoBaseId,
            fechaContratacion, salario, aerolineaId, usuarioId);

        await _repository.SaveAsync(worker);
        return worker;
    }
}
