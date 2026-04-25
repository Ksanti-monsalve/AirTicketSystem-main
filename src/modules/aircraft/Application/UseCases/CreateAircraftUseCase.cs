// src/modules/aircraft/Application/UseCases/CreateAircraftUseCase.cs
using AirTicketSystem.modules.aircraft.Domain.Repositories;
using AirTicketSystem.modules.aircraft.Domain.aggregate;
using AirTicketSystem.modules.aircraft.Domain.ValueObjects;
using AirTicketSystem.modules.aircraftmodel.Domain.Repositories;
using AirTicketSystem.modules.airline.Domain.Repositories;

namespace AirTicketSystem.modules.aircraft.Application.UseCases;

public class CreateAircraftUseCase
{
    private readonly IAircraftRepository _repository;
    private readonly IAircraftModelRepository _modelRepository;
    private readonly IAirlineRepository _airlineRepository;

    public CreateAircraftUseCase(
        IAircraftRepository repository,
        IAircraftModelRepository modelRepository,
        IAirlineRepository airlineRepository)
    {
        _repository       = repository;
        _modelRepository  = modelRepository;
        _airlineRepository = airlineRepository;
    }

    public async Task<Aircraft> ExecuteAsync(
        int modeloAvionId,
        int aerolineaId,
        string matricula,
        DateOnly? fechaFabricacion,
        DateOnly? fechaProximoMantenimiento,
        CancellationToken cancellationToken = default)
    {
        _ = await _modelRepository.FindByIdAsync(modeloAvionId)
            ?? throw new KeyNotFoundException(
                $"No se encontró un modelo de avión con ID {modeloAvionId}.");

        var aerolinea = await _airlineRepository.FindByIdAsync(aerolineaId)
            ?? throw new KeyNotFoundException(
                $"No se encontró una aerolínea con ID {aerolineaId}.");

        if (!aerolinea.Activa.Valor)
            throw new InvalidOperationException(
                $"La aerolínea '{aerolinea.Nombre.Valor}' está inactiva. " +
                "No se pueden registrar aviones para aerolíneas inactivas.");

        var matriculaVO = MatriculaAircraft.Crear(matricula);

        if (await _repository.ExistsByMatriculaAsync(matriculaVO.Valor))
            throw new InvalidOperationException(
                $"Ya existe un avión con la matrícula '{matriculaVO.Valor}'.");

        var aircraft = Aircraft.Crear(
            modeloAvionId,
            aerolineaId,
            matriculaVO.Valor,
            fechaFabricacion,
            fechaProximoMantenimiento);

        await _repository.SaveAsync(aircraft);
        return aircraft;
    }
}