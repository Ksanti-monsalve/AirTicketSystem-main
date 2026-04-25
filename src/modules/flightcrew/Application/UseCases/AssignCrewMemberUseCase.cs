// src/modules/flightcrew/Application/UseCases/AssignCrewMemberUseCase.cs
using AirTicketSystem.modules.flightcrew.Domain.Repositories;
using AirTicketSystem.modules.flightcrew.Domain.aggregate;
using AirTicketSystem.modules.flight.Domain.Repositories;
using AirTicketSystem.modules.worker.Domain.Repositories;
using AirTicketSystem.modules.pilotlicense.Domain.Repositories;

namespace AirTicketSystem.modules.flightcrew.Application.UseCases;

public sealed class AssignCrewMemberUseCase
{
    private readonly IFlightCrewRepository _repository;
    private readonly IFlightRepository _flightRepository;
    private readonly IWorkerRepository _workerRepository;
    private readonly IPilotLicenseRepository _licenseRepository;

    public AssignCrewMemberUseCase(
        IFlightCrewRepository repository,
        IFlightRepository flightRepository,
        IWorkerRepository workerRepository,
        IPilotLicenseRepository licenseRepository)
    {
        _repository        = repository;
        _flightRepository  = flightRepository;
        _workerRepository  = workerRepository;
        _licenseRepository = licenseRepository;
    }

    public async Task<FlightCrew> ExecuteAsync(
        int vueloId,
        int trabajadorId,
        string rolEnVuelo,
        CancellationToken cancellationToken = default)
    {
        var vuelo = await _flightRepository.FindByIdAsync(vueloId)
            ?? throw new KeyNotFoundException(
                $"No se encontró un vuelo con ID {vueloId}.");

        if (vuelo.Estado.Valor != "PROGRAMADO" && vuelo.Estado.Valor != "DEMORADO")
            throw new InvalidOperationException(
                $"No se puede asignar tripulación a un vuelo en estado " +
                $"'{vuelo.Estado}'.");

        var trabajador = await _workerRepository.FindByIdAsync(trabajadorId)
            ?? throw new KeyNotFoundException(
                $"No se encontró un trabajador con ID {trabajadorId}.");

        if (!trabajador.EstaActivo)
            throw new InvalidOperationException(
                "No se puede asignar un trabajador inactivo a un vuelo.");

        // Crear el aggregate — valida el rol internamente
        var crewMember = FlightCrew.Crear(vueloId, trabajadorId, rolEnVuelo);

        // Si es piloto o copiloto, verificar licencia vigente
        if (crewMember.EsParteDeCabina)
        {
            var licencias = await _licenseRepository.FindByTrabajadorAsync(trabajadorId);
            var hoy = DateOnly.FromDateTime(DateTime.Today);

            var tieneVigenteComercial = licencias.Any(l =>
                l.Activa.Valor &&
                l.FechaVencimiento.Valor >= hoy &&
                (l.TipoLicencia.Valor == "CPL" ||
                 l.TipoLicencia.Valor == "ATPL"));

            if (!tieneVigenteComercial)
                throw new InvalidOperationException(
                    "El trabajador no tiene una licencia CPL o ATPL vigente " +
                    "para operar como piloto o copiloto.");
        }

        if (await _repository.ExistsByVueloAndTrabajadorAsync(vueloId, trabajadorId))
            throw new InvalidOperationException(
                "El trabajador ya está asignado a este vuelo.");

        if (await _repository.ExistsByVueloAndRolAsync(vueloId, crewMember.RolEnVuelo.Valor))
            throw new InvalidOperationException(
                $"El rol '{crewMember.RolEnVuelo}' ya está ocupado en este vuelo.");

        await _repository.SaveAsync(crewMember);
        return crewMember;
    }
}